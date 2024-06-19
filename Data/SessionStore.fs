module Rommulbad.Data.SessionStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Session
open Rommulbad.Application.Session


type SessionStore(store: Store) =
    interface ISessionDataAccess with
        // list of all sessions from a candidate
        member this.get(name: string) : List<Session> =
            InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions
            |> Seq.map (fun (_, deep, date, minutes) ->
                { Deep = deep
                  Date = date
                  Minutes = minutes })
            |> List.ofSeq

        // add a session to a candidate
        member this.add (name: string) (session: Session) : Result<unit, string> =
            let result =
                InMemoryDatabase.insert
                    (name, session.Date)
                    (name, session.Deep, session.Date, session.Minutes)
                    store.sessions

            match result with
            | Ok _ -> Ok()
            | Error(UniquenessError message) -> failwith message

        // total minutes of all sessions from a candidate
        member this.total(name: string) : int =
            InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions
            |> Seq.map (fun (_, _, _, a) -> a)
            |> Seq.sum


        // list of all sessions from a candidate that are eligible for a diploma
        member this.eligibleSessions(name: string, diploma: string) : List<Session> =

            // TODO: custom type for diploma
            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            InMemoryDatabase.filter (fun (n, d, _, a) -> (d || shallowOk) && (a >= minMinutes)) store.sessions
            |> Seq.map (fun (_, deep, date, minutes) ->
                { Deep = deep
                  Date = date
                  Minutes = minutes })
            |> List.ofSeq

        // total minutes of all sessions from a candidate that are eligible for a diploma
        member this.eligibleSessionsTotal(name: string, diploma: string) : int =
            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let filter (n, d, _, a) = (d || shallowOk) && (a >= minMinutes)

            InMemoryDatabase.filter filter store.sessions
            |> Seq.map (fun (_, _, _, a) -> a)
            |> Seq.sum
