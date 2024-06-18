module Rommulbad.Service.Candidate.Candidate

open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe
open Rommulbad.Application.Candidate
open Microsoft.AspNetCore.Http

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
            | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
            | Some(candidate) -> return! ThothSerializer.RespondJson candidate Serialization.encode next ctx
        }


let handlers: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate ]
