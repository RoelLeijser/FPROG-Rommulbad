module Rommulbad.Application.Guardian

open Rommulbad.Model.Guardian

type IGuardianDataAccess =
    abstract all: unit -> Guardian list
    abstract get: string -> Guardian option
    abstract add: Guardian -> Result<unit, string>