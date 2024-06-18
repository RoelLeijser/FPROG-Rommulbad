module Rommulbad.Data.CandidateStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Candidate
open Rommulbad.Application.Candidate

type CandidateStore(store: Store) =
    interface ICandidateDataAccess with
        member this.all() : List<Candidate> =
            InMemoryDatabase.all store.candidates
            |> Seq.map (fun (name, _, gId, dpl) ->
                { Name = name
                  GuardianId = gId
                  Diploma = dpl })
            |> List.ofSeq
