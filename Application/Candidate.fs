module Rommulbad.Application.Candidate

open Rommulbad.Model.Candidate

type ICandidateDataAccess =
    abstract all: unit -> List<Candidate>
    abstract get: string -> Candidate option

let getAllCandidates (candidateDataStore: ICandidateDataAccess) : List<Candidate> = candidateDataStore.all ()

let getCandidate (candidateDataStore: ICandidateDataAccess) (name: string) : Candidate option =
    candidateDataStore.get name
