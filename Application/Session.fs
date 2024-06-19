module Rommulbad.Application.Session

open Rommulbad.Model.Session

type ISessionDataAccess =
    abstract member get: string -> List<Session>
    abstract member add: string -> Session -> Result<unit, string>
