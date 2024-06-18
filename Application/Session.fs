module Rommulbad.Application.Session

open Rommulbad.Model.Session

type ISessionDataAccess =
    abstract get: string -> Session option
