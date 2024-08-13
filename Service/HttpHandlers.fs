module Rommulbad.Service.HttpHandler

open Giraffe
open Rommulbad.Service.Candidate
open Rommulbad.Service.Session
open Rommulbad.Service.Guardian

let requestHandlers: HttpHandler = choose [ Candidate.handlers; Session.handlers; Guardian.handlers ]
