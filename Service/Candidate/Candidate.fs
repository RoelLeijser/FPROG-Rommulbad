module Rommulbad.Service.Candidate.Candidate

open Giraffe
open Thoth.Json.Giraffe
open Rommulbad.Application.Candidate
open Microsoft.AspNetCore.Http
open Rommulbad.Model.Candidate

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

let handlers: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate
          POST >=> route "/candidate" >=> addCandidate ]
