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

        member this.get(name: string) : Candidate option =
            InMemoryDatabase.lookup name store.candidates
            |> Option.map (fun (name, _, gId, dpl) ->
                { Name = name
                  GuardianId = gId
                  Diploma = dpl })

        member this.add(candidate: Candidate) =
            let insertResult = InMemoryDatabase.insert candidate.Name (candidate.Name, System.DateTime.Now, candidate.GuardianId, candidate.Diploma) store.candidates
            match insertResult with
            | Ok _ -> Ok()
            | Error err -> Error (err.ToString())
            
           
