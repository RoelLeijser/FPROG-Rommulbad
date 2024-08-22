module Rommulbad.Service.Candidate.Candidate

open Giraffe
open Thoth.Json.Giraffe
open Rommulbad.Application.Candidate
open Rommulbad.Application.Session
open Microsoft.AspNetCore.Http
open Rommulbad.Model.Candidate
open Rommulbad.Model.Diploma

let getCandidates (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<ICandidateDataAccess>()
        let candidates = dataAccess.all ()

        return! ThothSerializer.RespondJsonSeq candidates Serialization.encode next ctx
    }

let getCandidate (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ICandidateDataAccess>()
            let candidate = dataAccess.get name

            match candidate with
            | None -> return! RequestErrors.NOT_FOUND "Candidate not found!" next ctx
            | Some(candidate) -> return! ThothSerializer.RespondJson candidate Serialization.encode next ctx
        }

let addCandidate : HttpHandler =
    fun next ctx ->
        task {
            let! candidate = ThothSerializer.ReadBody ctx Serialization.decode 

            match candidate with
            | Error msg -> return! RequestErrors.BAD_REQUEST msg next ctx
            | Ok candidate ->
                let dataAccess = ctx.GetService<ICandidateDataAccess>()
                let result = dataAccess.add candidate

                match result with
                | Error msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                | Ok () ->
                    return! ThothSerializer.RespondJson candidate Serialization.encode next ctx
        }

let awardDiploma (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let candidateDataAccess = ctx.GetService<ICandidateDataAccess>()
            let sessionDataAccess = ctx.GetService<ISessionDataAccess>()
            let candidate = candidateDataAccess.get name
            
            match candidate with
            | None -> return! RequestErrors.NOT_FOUND "Candidate not found!" next ctx
            | Some(candidate) ->
                let sessions = sessionDataAccess.get candidate.Name
                let eligibleSessions = eligibleSessions sessions diploma
                let total = totalMinutes eligibleSessions
                let diploma = Diploma.make diploma

                if total >= Diploma.totalMinutes diploma then
                    candidateDataAccess.update { candidate with Diploma = diploma } 
                    
                    return! ThothSerializer.RespondJson candidate Serialization.encode next ctx
                else
                    return! RequestErrors.BAD_REQUEST "Not enough minutes for diploma" next ctx
        }

let handlers: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate
          POST >=> route "/candidate" >=> addCandidate
          POST >=> routef "/candidate/%s/award/%s" awardDiploma
        ]
