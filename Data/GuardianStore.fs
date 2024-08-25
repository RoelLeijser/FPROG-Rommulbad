module Rommulbad.Data.GuardianStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Guardian
open Rommulbad.Application.Guardian
open Rommulbad.Model.Candidate
open Rommulbad.Model.Diploma
open Rommulbad.Model.Common

let guardianIdtoString (id: GuardianId) = id |> fun (GuardianId id) -> id

let getAllCandidatesByGuardianId (guardianId: string) (store: Store) =
    InMemoryDatabase.all store.candidates
    |> Seq.filter (fun (_, _, gId, _) -> gId = guardianId)
    |> Seq.map (fun (name, dob, _, dpl) ->
        { Name = name
          GuardianId = GuardianId guardianId
          DateOfBirth = dob
          Diploma = Diploma.make dpl })
    |> List.ofSeq


type GuardianStore(store: Store) =
    interface IGuardianDataAccess with
        member this.all() =
            InMemoryDatabase.all store.guardians
            |> Seq.map (fun (id, name) ->
                { Id = GuardianId id
                  Name = name
                  Candidates = getAllCandidatesByGuardianId id store })
            |> List.ofSeq


        member this.get(id: string) : Guardian option =
            InMemoryDatabase.lookup id store.guardians
            |> Option.map (fun (id, name) ->
                { Id = GuardianId id
                  Name = name
                  Candidates = getAllCandidatesByGuardianId id store })


        member this.add(guardian: Guardian) =
            let insertResult =
                  InMemoryDatabase.insert
                    (guardianIdtoString guardian.Id)
                    ((guardianIdtoString guardian.Id), guardian.Name)
                    store.guardians

            match insertResult with
            | Ok _ -> Ok()
            | Error err -> Error(err.ToString())
