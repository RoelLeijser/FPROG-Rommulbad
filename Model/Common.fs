module Rommulbad.Model.Common

open System.Text.RegularExpressions
open Rommulbad.Model.Validation

type GuardianId = GuardianId of string

let (|GuardianId|) (GuardianId guardianId) = guardianId

module GuardianId =
    let make rawGuardianId =
        rawGuardianId
        |> nonEmpty "Guardian ID cannot be empty"
        |> Result.bind (matches (Regex("^\d{3}-[A-Z]{4}$")) "Guardian ID must be in the format '###-AAAA'")
        |> Result.map GuardianId

type SessionMinutes = SessionMinutes of int

let (|SessionMinutes|) (SessionMinutes minutes) = minutes

module SessionMinutes =
    let make rawMinutes =
        rawMinutes
        |> isNotNegative "Minutes cannot be negative"
        |> Result.bind (lessThanOrEqual "Minutes cannot be larger than 30" 30)
        |> Result.map SessionMinutes
