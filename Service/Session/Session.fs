module Rommulbad.Service.Session.Session

open Giraffe
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Rommulbad.Application.Session
open Thoth.Json.Net
open Rommulbad.Model.Session
open Rommulbad.Model.Diploma

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

            let eligibleSessions =
                sessions
                |> Seq.filter (fun (session: Session) ->
                    session.Minutes >= Diploma.minMinutes (Diploma.make diploma)
                    && session.Deep = Diploma.shallowOk (Diploma.make diploma))

            return! ThothSerializer.RespondJsonSeq eligibleSessions Serialization.encode next ctx
        }

let getTotalEligibleMinutes (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = dataAccess.get name

            let eligibleSessions =
                sessions
                |> Seq.filter (fun (session: Session) ->
                    session.Minutes >= Diploma.minMinutes (Diploma.make diploma)
                    && session.Deep = Diploma.shallowOk (Diploma.make diploma))

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
