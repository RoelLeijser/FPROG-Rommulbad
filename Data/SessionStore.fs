module Rommulbad.Data.SessionStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Session
open Rommulbad.Application.Session
open Rommulbad.Model.Common

let sessionMinutesToInt (SessionMinutes minutes) = minutes

type SessionStore(store: Store) =
    interface ISessionDataAccess with
        // list of all sessions from a candidate
        member this.get(name: string) : List<Session> =
            InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions
            |> Seq.map (fun (_, deep, date, minutes) ->
                { Deep = deep
                  Date = date
                  Minutes = SessionMinutes minutes })
            |> List.ofSeq

        // add a session to a candidate
        member this.add (name: string) (session: Session) : Result<unit, string> =
            let result =
                InMemoryDatabase.insert
                    (name, session.Date)
                    (name, session.Deep, session.Date, sessionMinutesToInt session.Minutes)
                    store.sessions

            match result with
            | Ok _ -> Ok()
            | Error(UniquenessError message) -> failwith message