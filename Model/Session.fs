module Rommulbad.Model.Session

open System
open Common
open Thoth.Json.Net


/// Swimming session registered on a specific date
///
/// A Swimming session can be in the deep or shallow pool
/// Minutes cannot be negative or larger than 30
/// Only the year, month and date of Date are used.
type Session =
    { Deep: bool
      Date: DateTime
      Minutes: SessionMinutes }


module Session =
  
  let encodeSessionMinutes: Encoder<int> = fun minutes -> Encode.int minutes

  