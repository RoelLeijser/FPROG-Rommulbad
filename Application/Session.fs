module Rommulbad.Application.Session

open Rommulbad.Model.Session
open Rommulbad.Model.Diploma

type ISessionDataAccess =
    abstract member get: string -> List<Session>
    abstract member add: string -> Session -> Result<unit, string>

let eligibleSessions (sessions: List<Session>) (diploma: string) =
        let diploma = Diploma.make diploma

        sessions
            |> List.filter (fun session -> session.Deep || Diploma.shallowOk diploma)
            |> List.filter (fun session -> session.Minutes >= Diploma.minMinutes diploma)
            
let totalMinutes (sessions: List<Session>) =
    Seq.sumBy (fun (session: Session) -> session.Minutes) sessions