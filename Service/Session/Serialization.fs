module Rommulbad.Service.Session.Serialization

open Thoth.Json.Net
open Rommulbad.Model.Session
open Rommulbad.Model.Common

let encodeSessionMinutes: Encoder<int> = fun minutes -> Encode.int minutes
let SessionMinutesToInt (SessionMinutes minutes) = minutes

let decodeSessionMinutes =
    Decode.int
    |> Decode.andThen (fun minutes ->
        match SessionMinutes.make minutes with
        | Ok minutes -> Decode.succeed minutes
        | Error msg -> Decode.fail msg)

let encode: Encoder<Session> =
    fun session ->
        Encode.object
            [ "deep", Encode.bool session.Deep
              "date", Encode.datetime session.Date
              "amount", Encode.int (SessionMinutesToInt session.Minutes) ]

let decode: Decoder<Session> =
    Decode.object (fun get ->
        { Deep = get.Required.Field "deep" Decode.bool
          Date = get.Required.Field "date" Decode.datetime
          Minutes = get.Required.Field "amount" decodeSessionMinutes })
