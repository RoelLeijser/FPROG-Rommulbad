module Rommulbad.Data.SessionStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Session
open Rommulbad.Model.Candidate
open Rommulbad.Application.Session


type SessionStore(store: Store) =
    interface ISessionDataAccess with
        // list of all sessions from a candidate
        member this.get(name: string) =
            InMemoryDatabase.all store.sessions
            |> Seq.tryFind (fun (n, _, _, _) -> n = name)
            |> Option.map (fun (_, deep, date, minutes) ->
                { Deep = deep
                  Date = date
                  Minutes = minutes })
