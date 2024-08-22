module Rommulbad.Service.Guardian.Serialization

open Rommulbad.Model.Guardian
open Rommulbad.Model.Common

open Thoth.Json.Net

let encodeId: Encoder<GuardianId> = fun (GuardianId id) -> Encode.string id
let decodeId: Decoder<GuardianId> = Decode.string |> Decode.map GuardianId


let encode: Encoder<Guardian> =
    fun guardian ->
        Encode.object
            [ "id", encodeId guardian.Id
              "name", Encode.string guardian.Name
              "candidates",
              Encode.list (guardian.Candidates |> List.map Rommulbad.Service.Candidate.Serialization.encode) ]

let decode: Decoder<Guardian> =
    Decode.object (fun get ->
        { Id = get.Required.Field "id" decodeId
          Name = get.Required.Field "name" Decode.string
          Candidates =
            get.Optional.Field "candidates" (Decode.list Rommulbad.Service.Candidate.Serialization.decode)
            |> Option.defaultValue [] })
