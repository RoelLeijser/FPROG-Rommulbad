module Rommulbad.Application.Session

open Rommulbad.Model.Session
open Rommulbad.Model.Diploma
open Rommulbad.Model.Common

type ISessionDataAccess =
    abstract member get: string -> List<Session>
    abstract member add: string -> Session -> Result<unit, string>

let get (store: ISessionDataAccess) (name: string) =  
    store.get name  

let add (store: ISessionDataAccess) (name: string) (session: Session) =
    store.add name session

let sessionMinuteToInt (sessionMinute: SessionMinutes) = 
    match sessionMinute with
    | SessionMinutes minutes -> minutes

let eligibleSessions (sessions: List<Session>) (diploma: string) =
        let diploma = Diploma.make diploma

        sessions
            |> List.filter (fun session -> session.Deep || Diploma.shallowOk diploma)
            |> List.filter (fun session -> session.Minutes >= SessionMinutes (Diploma.minMinutes diploma))
            
let totalMinutes (sessions: List<Session>) =
    Seq.sumBy (fun (session: Session) -> sessionMinuteToInt session.Minutes) sessions

let isEligible (sessions: List<Session>) (diploma: string) =
    let eligibleSessions = eligibleSessions sessions diploma
    let total = totalMinutes eligibleSessions
    let diploma = Diploma.make diploma

    total >= Diploma.totalMinutes diploma