module Rommulbad.Service.Session.Serialization

open Thoth.Json.Net
open Rommulbad.Model.Session

let encode: Encoder<Session> =
    fun session ->
        Encode.object
            [ "deep", Encode.bool session.Deep
              "date", Encode.datetime session.Date
              "amount", Encode.int session.Minutes ]

let decode: Decoder<Session> =
    Decode.object (fun get ->
        { Deep = get.Required.Field "deep" Decode.bool
          Date = get.Required.Field "date" Decode.datetime
          Minutes = get.Required.Field "amount" Decode.int })
