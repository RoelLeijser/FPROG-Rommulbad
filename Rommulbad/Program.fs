﻿open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Rommulbad.Data.Store
open Rommulbad.Service.HttpHandler
open Rommulbad.Application.Candidate
open Rommulbad.Data.CandidateStore
open Rommulbad.Data.SessionStore
open Rommulbad.Data.GuardianStore
open Rommulbad.Application.Session
open Rommulbad.Application.Guardian


let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe requestHandlers

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services
        .AddGiraffe()
        .AddSingleton<Store>(Store())
        .AddSingleton<ICandidateDataAccess, CandidateStore>()
        .AddSingleton<ISessionDataAccess, SessionStore>()
        .AddSingleton<IGuardianDataAccess, GuardianStore>()
        .AddSingleton<Json.ISerializer>(ThothSerializer(skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
    |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder.Configure(configureApp).ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
