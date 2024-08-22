module Rommulbad.Application.Candidate

open Rommulbad.Model.Candidate

type ICandidateDataAccess =
    abstract all: unit -> List<Candidate>
    abstract get: string -> Candidate option
    abstract add: Candidate -> Result<unit, string>
    abstract update: Candidate  -> unit
