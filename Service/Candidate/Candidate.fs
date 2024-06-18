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
            // let store = ctx.GetService<Store>()

            // let candidate = InMemoryDatabase.lookup name store.candidates


            // match candidate with
            // | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
            // | Some(name, _, gId, dpl) ->
            //     return!
            //         ThothSerializer.RespondJson
            //             { Name = name
            //               GuardianId = gId
            //               Diploma = dpl }
            //             Candidate.encode
            //             next
            //             ctx
            return! text "OK" next ctx
        }


let handlers: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate ]
