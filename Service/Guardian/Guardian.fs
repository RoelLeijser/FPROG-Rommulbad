module Rommulbad.Service.Guardian.Guardian

open Giraffe
open Thoth.Json.Giraffe
open Rommulbad.Application.Guardian
open Microsoft.AspNetCore.Http
open Rommulbad.Model.Guardian
let getGuardians (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IGuardianDataAccess>()
        let guardians = getAll dataAccess

        return! ThothSerializer.RespondJsonSeq guardians Serialization.encode next ctx
    }

let getGuardian (id: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IGuardianDataAccess>()
            let guardian = get dataAccess id

            match guardian with
            | None -> return! RequestErrors.NOT_FOUND "Guardian not found!" next ctx
            | Some(guardian) -> return! ThothSerializer.RespondJson guardian Serialization.encode next ctx
        }

let addGuardian : HttpHandler =
    fun next ctx ->
        task {
            let! guardian = ThothSerializer.ReadBody ctx Serialization.decode 

            match guardian with
            | Error msg -> return! RequestErrors.BAD_REQUEST msg next ctx
            | Ok guardian ->
                let dataAccess = ctx.GetService<IGuardianDataAccess>()
                let result = add dataAccess guardian

                match result with
                | Error msg -> return! RequestErrors.BAD_REQUEST msg next ctx
                | Ok () ->
                    return! ThothSerializer.RespondJson guardian Serialization.encode next ctx
        }

let handlers: HttpHandler =
    choose
        [ GET >=> route "/guardian" >=> getGuardians
          GET >=> routef "/guardian/%s" getGuardian
          POST >=> route "/guardian" >=> addGuardian ]