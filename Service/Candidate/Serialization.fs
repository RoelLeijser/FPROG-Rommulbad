module Rommulbad.Service.Candidate.Serialization

open Rommulbad.Model.Candidate
open Rommulbad.Model.Diploma

open Thoth.Json.Net
open Rommulbad.Model.Common

let encodeDiploma: Encoder<Diploma> =
    (fun (Diploma diploma) -> Encode.string diploma)

let encodeGuardianId: Encoder<GuardianId> = fun (GuardianId id) -> Encode.string id
let decodGuardianId: Decoder<GuardianId> =
    Decode.string
    |> Decode.andThen (fun id ->
        match GuardianId.make id with
        | Ok id -> Decode.succeed id
        | Error msg -> Decode.fail msg)

let encode: Encoder<Candidate> =
    fun candidate ->
        Encode.object
            [ "name", Encode.string candidate.Name
              "guardian_id", encodeGuardianId candidate.GuardianId
              "diploma", Encode.option Diploma.encode candidate.Diploma
              "date_of_birth", Encode.datetime candidate.DateOfBirth ]

let decode: Decoder<Candidate> =
    Decode.object (fun get ->
        { Name = get.Required.Field "name" Decode.string
          GuardianId = get.Required.Field "guardian_id" decodGuardianId
          DateOfBirth = get.Required.Field "date_of_birth" Decode.datetime
          Diploma = None })
