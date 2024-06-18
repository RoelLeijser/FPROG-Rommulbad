module Rommulbad.Application.Candidate

open Rommulbad.Model.Candidate

type ICandidateDataAccess =
    abstract all: unit -> List<Candidate>

let getAllCandidates (candidateDataStore: ICandidateDataAccess) : List<Candidate> = candidateDataStore.all ()
