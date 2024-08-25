module Rommulbad.Application.Candidate

open Rommulbad.Model.Candidate

type ICandidateDataAccess =
    abstract all: unit -> List<Candidate>
    abstract get: string -> Candidate option
    abstract add: Candidate -> Result<unit, string>
    abstract update: Candidate  -> unit

let all (store: ICandidateDataAccess) =
    store.all()

let get (store: ICandidateDataAccess) (name: string) =
    store.get name

let add (store: ICandidateDataAccess) (candidate: Candidate) =
    store.add candidate

let update (store: ICandidateDataAccess) (candidate: Candidate) =
    store.update candidate