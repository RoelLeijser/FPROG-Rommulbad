module Rommulbad.Service.Session.Session

open Giraffe
open Thoth.Json.Giraffe
open Rommulbad.Application.Session
open Thoth.Json.Net
open Rommulbad.Model.Common
open Rommulbad.Model.Session

let encodeSessionMinutes: Encoder<int> = fun minutes -> Encode.int minutes

let addSession (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let! session = ThothSerializer.ReadBody ctx Serialization.decode

            match session with
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Ok session ->
                let result = add dataAccess name session

                match result with
                | Ok _ -> return! text "OK" next ctx
                | Error message -> return! RequestErrors.BAD_REQUEST message next ctx
        }

let getSessions (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = get dataAccess name

            return! ThothSerializer.RespondJsonSeq sessions Serialization.encode next ctx
        }

let getTotalMinutes (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = get dataAccess name
            
            return! ThothSerializer.RespondJson (totalMinutes sessions) encodeSessionMinutes next ctx
        }


let getEligibleSessions (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = get dataAccess name

            return! ThothSerializer.RespondJsonSeq (eligibleSessions sessions diploma) Serialization.encode next ctx
        }

let getTotalEligibleMinutes (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<ISessionDataAccess>()
            let sessions = get dataAccess name

            return! ThothSerializer.RespondJson (totalMinutes (eligibleSessions sessions diploma)) encodeSessionMinutes next ctx
        }

let handlers: HttpHandler =
    choose
        [ POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          GET >=> routef "/candidate/%s/session/%s" getEligibleSessions
          GET >=> routef "/candidate/%s/session/%s/total" getTotalEligibleMinutes ]
