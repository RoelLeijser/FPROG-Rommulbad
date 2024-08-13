module Rommulbad.Data.GuardianStore

open Rommulbad.Data.Store
open Rommulbad.Database
open Rommulbad.Model.Guardian
open Rommulbad.Application.Guardian
open Rommulbad.Model.Candidate

let getAllCandidatesByGuardianId (guardianId: string) (store: Store) =
    InMemoryDatabase.all store.candidates
    |> Seq.filter (fun (_, _, gId, _) -> gId = guardianId)
    |> Seq.map (fun (name, _, _, dpl) -> 
        { Name = name
          GuardianId = guardianId
          Diploma = dpl })
    |> List.ofSeq


type GuardianStore(store: Store) =
    interface IGuardianDataAccess with
        member this.all() =
            InMemoryDatabase.all store.guardians
            |> Seq.map (fun (id, name) -> 
                { Id = id
                  Name = name
                  Candidates = getAllCandidatesByGuardianId id store })
            |> List.ofSeq
            
            
        member this.get(id: string): Guardian option = 
            InMemoryDatabase.lookup id store.guardians
            |> Option.map (fun (id, name) -> 
                { Id = id
                  Name = name
                  Candidates = getAllCandidatesByGuardianId id store })
            

        member this.add(guardian: Guardian): Result<unit,string> = 
            let insertResult = InMemoryDatabase.insert guardian.Id (guardian.Id, guardian.Name) store.guardians
            match insertResult with
            | Ok _ -> Ok()
            | Error err -> Error (err.ToString())