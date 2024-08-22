module Rommulbad.Data.CandidateStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Candidate
open Rommulbad.Application.Candidate
open Rommulbad.Model.Diploma

type CandidateStore(store: Store) =
    interface ICandidateDataAccess with
        member this.all() : List<Candidate> =
            InMemoryDatabase.all store.candidates
            |> Seq.map (fun (name, _, gId, dpl) ->
                { Name = name
                  GuardianId = gId
                  Diploma = Diploma.make dpl })
            |> List.ofSeq

        member this.get(name: string) : Candidate option =
            InMemoryDatabase.lookup name store.candidates
            |> Option.map (fun (name, _, gId, dpl) ->
                { Name = name
                  GuardianId = gId
                  Diploma = Diploma.make dpl })

        member this.add(candidate: Candidate) =
            match candidate.Diploma with
            | Diploma diploma ->
                let insertResult =
                    InMemoryDatabase.insert
                        candidate.Name
                        (candidate.Name, System.DateTime.Now, candidate.GuardianId, diploma)
                        store.candidates

                match insertResult with
                | Ok _ -> Ok()
                | Error err -> Error(err.ToString())

        member this.update (candidate: Candidate) = 
            match candidate.Diploma with
            | Diploma diploma ->
                InMemoryDatabase.update
                    candidate.Name
                    (candidate.Name, System.DateTime.Now, candidate.GuardianId, diploma)
                    store.candidates

