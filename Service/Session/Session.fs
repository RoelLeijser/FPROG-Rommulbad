module Rommulbad.Service.Session.Session

open Giraffe
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Rommulbad.Application.Session

let addSession (name: string) : HttpHandler =
    fun next ctx ->
        task {
            // let! session = ThothSerializer.ReadBody ctx Session.decode

            // match session with
            // | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            // | Ok { Deep = deep
            //        Date = date
            //        Minutes = minutes } ->
            //     let store = ctx.GetService<Store>()

            //     InMemoryDatabase.insert (name, date) (name, deep, date, minutes) store.sessions
            //     |> ignore


            return! text "OK" next ctx
        }

let getSessions (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name

            match sessions with
            | Some session -> return! ThothSerializer.RespondJson session Serialization.encode next ctx
            | None -> return! RequestErrors.NOT_FOUND "Candidate not found" next ctx
        }

let getTotalMinutes (name: string) : HttpHandler =
    fun next ctx ->
        task {
            // let store = ctx.GetService<Store>()

            // let total =
            //     InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions
            //     |> Seq.map (fun (_, _, _, a) -> a)
            //     |> Seq.sum

            // return! ThothSerializer.RespondJson total Encode.int next ctx
            return! text "OK" next ctx
        }


let getEligibleSessions (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            // let store = ctx.GetService<Store>()

            // let shallowOk =
            //     match diploma with
            //     | "A" -> true
            //     | _ -> false

            // let minMinutes =
            //     match diploma with
            //     | "A" -> 1
            //     | "B" -> 10
            //     | _ -> 15

            // let filter (n, d, _, a) = (d || shallowOk) && (a >= minMinutes)


            // let sessions = InMemoryDatabase.filter filter store.sessions

            // return! ThothSerializer.RespondJsonSeq sessions encodeSession next ctx
            return! text "OK" next ctx
        }

let getTotalEligibleMinutes (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            // let store = ctx.GetService<Store>()

            // let shallowOk =
            //     match diploma with
            //     | "A" -> true
            //     | _ -> false

            // let minMinutes =
            //     match diploma with
            //     | "A" -> 1
            //     | "B" -> 10
            //     | _ -> 15

            // let filter (n, d, _, a) = (d || shallowOk) && (a >= minMinutes)


            // let total =
            //     InMemoryDatabase.filter filter store.sessions
            //     |> Seq.map (fun (_, _, _, a) -> a)
            //     |> Seq.sum

            // return! ThothSerializer.RespondJson total Encode.int next ctx
            return! text "OK" next ctx
        }

let handlers: HttpHandler =
    choose
        [ POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          GET >=> routef "/candidate/%s/session/%s" getEligibleSessions
          GET >=> routef "/candidate/%s/session/%s/total" getTotalEligibleMinutes ]
