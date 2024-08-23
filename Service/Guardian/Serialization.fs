module Rommulbad.Service.Guardian.Serialization

open Rommulbad.Model.Guardian
open Rommulbad.Model.Common

open Thoth.Json.Net

let encodeGuardianId: Encoder<GuardianId> = fun (GuardianId id) -> Encode.string id
let decodGuardianId: Decoder<GuardianId> =
    Decode.string
    |> Decode.andThen (fun id ->
        match GuardianId.make id with
        | Ok id -> Decode.succeed id
        | Error msg -> Decode.fail msg)

let encode: Encoder<Guardian> =
    fun guardian -> 
        Encode.object [
            "id", encodeGuardianId guardian.Id
            "name", Encode.string guardian.Name
            "candidates", Encode.list (guardian.Candidates |> List.map Rommulbad.Service.Candidate.Serialization.encode ) ]  


let decode: Decoder<Guardian> = 
    Decode.object (fun get ->
        { Id = get.Required.Field "id" decodGuardianId
          Name = get.Required.Field "name" Decode.string
          Candidates = get.Optional.Field "candidates" (Decode.list Rommulbad.Service.Candidate.Serialization.decode) |> Option.defaultValue [] }) 
