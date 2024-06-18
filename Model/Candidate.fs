module Rommulbad.Model.Candidate

// Names are words separated by spaces
// GuardianId must be a valid guardian id (see below)
// Diploma is the highest diploma obtained by the candidate. It can be
// - an empty string meaning no diploma
// - or the strings "A", "B", or "C".
type Candidate =
    { Name: string
      GuardianId: string
      Diploma: string }
