module Rommulbad.Model.Session

open System

open Thoth.Json.Net

/// Swimming session registered on a specific date
///
/// A Swimming session can be in the deep or shallow pool
/// Minutes cannot be negative or larger than 30
/// Only the year, month and date of Date are used.
type Session =
    { Deep: bool
      Date: DateTime
      Minutes: int }

module Session =
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
