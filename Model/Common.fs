module Rommulbad.Model.Common

open System.Text.RegularExpressions
open Rommulbad.Model.Validation

type GuardianId = GuardianId of string

let (|GuardianId|) (GuardianId id) = id

[<RequireQualifiedAccess>]
module GuardianId =
    let guardianIdRegex = Regex "^\d{3}-[A-Z]{4}$"

    let make rawId =
        rawId
        |> nonEmpty "Guardian ID may not be empty."
        |> Result.bind (matches guardianIdRegex "Guardian ID must be 3 digits followed by a dash and 4 capital letters")
        |> Result.bind (alphaNumeric "Bin identifier may contain only letters or digits.")
        |> Result.map GuardianId
