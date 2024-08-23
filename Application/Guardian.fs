module Rommulbad.Application.Guardian

open Rommulbad.Model.Guardian
open Rommulbad.Model.Common

type IGuardianDataAccess =
    abstract all: unit -> Guardian list
    abstract get: string -> Guardian option
    abstract add: Guardian -> Result<unit, string>

let getAll (dataAccess: IGuardianDataAccess) = dataAccess.all ()

let get (dataAccess: IGuardianDataAccess) (id: string) = dataAccess.get id

let add (dataAccess: IGuardianDataAccess) (guardian: Guardian): Result<unit, string> =
    dataAccess.add guardian

