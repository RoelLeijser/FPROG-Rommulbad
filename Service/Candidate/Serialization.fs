module Rommulbad.Service.Candidate.Serialization

open Rommulbad.Model.Candidate

open Thoth.Json.Net

let encode: Encoder<Candidate> =
    fun candidate ->
        Encode.object
            [ "name", Encode.string candidate.Name
              "guardian_id", Encode.string candidate.GuardianId
              "diploma", Encode.string candidate.Diploma ]

let decode: Decoder<Candidate> =
    Decode.object (fun get ->
        { Name = get.Required.Field "name" Decode.string
          GuardianId = get.Required.Field "guardian_id" Decode.string
          Diploma = get.Required.Field "diploma" Decode.string })
