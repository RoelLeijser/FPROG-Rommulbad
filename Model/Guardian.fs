module Rommulbad.Model.Guardian

open Candidate
open Rommulbad.Model.Common

/// A guardian has an Id (3 digits followed by a dash and 4 letters),
/// a Name (only letters and spaces, but cannot contain two or more consecutive spaces),
/// and a list of Candidates (which may be empty)
type Guardian =
    { Id: GuardianId 
      Name: string
      Candidates: List<Candidate> }

module Guardian =
  let make id name candidates =
    { Id = id
      Name = name
      Candidates = candidates }