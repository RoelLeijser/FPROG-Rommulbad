module Rommulbad.Data.CandidateStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Candidate
open Rommulbad.Application.Candidate
open Rommulbad.Model.Diploma
open Rommulbad.Model.Common

let guardianIdtoString (id: GuardianId) = id |> fun (GuardianId id) -> id

type CandidateStore(store: Store) =
    interface ICandidateDataAccess with
        member this.all() : List<Candidate> =
            InMemoryDatabase.all store.candidates
            |> Seq.map (fun (name, dob, gId, dpl) ->
                { Name = name
                  GuardianId = GuardianId gId
                  DateOfBirth = dob
                  Diploma = Diploma.make dpl })
            |> List.ofSeq

        member this.get(name: string) : Candidate option =
            InMemoryDatabase.lookup name store.candidates
            |> Option.map (fun (name, dob, gId, dpl) ->
                { Name = name
                  GuardianId = GuardianId gId
                  DateOfBirth = dob
                  Diploma = Diploma.make dpl })

        member this.add(candidate: Candidate) =
            let result = InMemoryDatabase.insert candidate.Name (candidate.Name, candidate.DateOfBirth, guardianIdtoString candidate.GuardianId, "") store.candidates
            match result with
            | Ok _ -> Ok ()
            | Error error -> Error (error.ToString ())

        member this.update (candidate: Candidate) = 
            match candidate.Diploma with
            | Some diploma -> InMemoryDatabase.update candidate.Name (candidate.Name, candidate.DateOfBirth, guardianIdtoString candidate.GuardianId, diploma.ToString()) store.candidates
                            |> Ok 
            | None -> Error "Diploma is should be A, B or C"
