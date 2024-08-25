module Rommulbad.Service.Candidate.Candidate

open Giraffe
open Thoth.Json.Giraffe
open Rommulbad.Application.Session

open Rommulbad.Application.Candidate
open Microsoft.AspNetCore.Http
open Rommulbad.Model.Candidate
open Rommulbad.Model.Diploma

let getCandidates (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<ICandidateDataAccess>()
        let candidates = all dataAccess 

        return! ThothSerializer.RespondJsonSeq candidates Serialization.encode next ctx
    }

let getCandidate (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ICandidateDataAccess>()
            let candidate = get dataAccess name

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
                let result = add dataAccess candidate

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
            let candidate = get candidateDataAccess name
            
            match candidate with
            | None -> return! RequestErrors.NOT_FOUND "Candidate not found!" next ctx
            | Some(candidate) ->
                let sessions = Rommulbad.Application.Session.get sessionDataAccess candidate.Name

                let isEligible = isEligible sessions diploma

                match isEligible with
                | true ->
                    let diploma = Diploma.make diploma
                    let updatedCandidate = { candidate with Diploma = diploma }
                    let result = candidateDataAccess.update updatedCandidate
                    match result with
                    | Error msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                    | Ok () -> return! ThothSerializer.RespondJson updatedCandidate Serialization.encode next ctx

                | false -> return! RequestErrors.BAD_REQUEST "Not enough minutes for diploma" next ctx
        }

let getCandidatesEligibleFor (diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let candidateDataAccess = ctx.GetService<ICandidateDataAccess>()
            let sessionDataAccess = ctx.GetService<ISessionDataAccess>()
            let candidates = candidateDataAccess.all ()
            let eligibleCandidates = 
                candidates
                |> List.filter (fun candidate ->
                    let sessions = sessionDataAccess.get candidate.Name
                    isEligible sessions diploma
                )

            return! ThothSerializer.RespondJsonSeq eligibleCandidates Serialization.encode next ctx
        }

let handlers: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate
          POST >=> route "/candidate" >=> addCandidate
          POST >=> routef "/candidate/%s/award/%s" awardDiploma
          GET >=> routef "/candidate/eligible/%s" getCandidatesEligibleFor
        ]
