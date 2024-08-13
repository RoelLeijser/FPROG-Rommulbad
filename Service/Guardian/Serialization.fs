module Rommulbad.Service.Guardian.Serialization

open Rommulbad.Model.Guardian
open Rommulbad.Service.Candidate.Serialization

open Thoth.Json.Net

let encode: Encoder<Guardian> =
    fun guardian -> 
        Encode.object [
            "id", Encode.string guardian.Id
            "name", Encode.string guardian.Name
            "candidates", Encode.list (guardian.Candidates |> List.map Rommulbad.Service.Candidate.Serialization.encode ) ]  

let decode: Decoder<Guardian> = 
    Decode.object (fun get ->
        { Id = get.Required.Field "id" Decode.string
          Name = get.Required.Field "name" Decode.string
          Candidates = get.Required.Field "candidates" (Decode.list Rommulbad.Service.Candidate.Serialization.decode) })