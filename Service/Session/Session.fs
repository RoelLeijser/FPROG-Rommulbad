module Rommulbad.Service.Session.Session

open Giraffe
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Rommulbad.Application.Session
open Thoth.Json.Net
open Rommulbad.Model.Session

let addSession (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let! session = ThothSerializer.ReadBody ctx Serialization.decode

            match session with
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Ok session ->
                let result = dataAccess.add name session

                match result with
                | Ok _ -> return! text "OK" next ctx
                | Error message -> return! RequestErrors.BAD_REQUEST message next ctx
        }

let getSessions (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name

            return! ThothSerializer.RespondJsonSeq sessions Serialization.encode next ctx
        }

let getTotalMinutes (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name
            let total = Seq.sumBy (fun (session: Session) -> session.Minutes) sessions

            return! ThothSerializer.RespondJson total Encode.int next ctx
        }


let getEligibleSessions (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name

            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let eligibleSessions = sessions 
                                    |> Seq.filter (fun (session: Session)
                                                    -> session.Minutes >= minMinutes
                                                    && session.Deep = shallowOk)

            return! ThothSerializer.RespondJsonSeq eligibleSessions Serialization.encode next ctx
        }

let getTotalEligibleMinutes (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name

            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let eligibleSessions = sessions 
                                    |> Seq.filter (fun (session: Session)
                                                    -> session.Minutes >= minMinutes
                                                    && session.Deep = shallowOk)

            let total = Seq.sumBy (fun (session: Session) -> session.Minutes) eligibleSessions
            
            return! ThothSerializer.RespondJson total Encode.int next ctx
        }

let handlers: HttpHandler =
    choose
        [ POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          GET >=> routef "/candidate/%s/session/%s" getEligibleSessions
          GET >=> routef "/candidate/%s/session/%s/total" getTotalEligibleMinutes ]
