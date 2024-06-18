module Rommulbad.Service.HttpHandler

open Giraffe
open Rommulbad.Service.Candidate
open Rommulbad.Service.Session

let requestHandlers: HttpHandler = choose [ Candidate.handlers; Session.handlers ]
