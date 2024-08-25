module Rommulbad.Model.Diploma

open Thoth.Json.Net

type Diploma =
    private
    | A
    | B
    | C

let (|Diploma|) =
    function
    | A -> "A"
    | B -> "B"
    | C -> "C"


module Diploma =
    let make rawDiploma =
        match rawDiploma with
        | "A" -> Some A
        | "B" -> Some B
        | "C" -> Some C
        | _ -> None

    let shallowOk (diploma: Diploma) =
        match diploma with
        | A -> true
        | _ -> false

    let minMinutes (diploma: Diploma) =
        match diploma with
        | A -> 1
        | B -> 10
        | C -> 15

    let totalMinutes (diploma: Diploma) =
        match diploma with
        | A -> 120
        | B -> 150
        | C -> 180

    let encode: Encoder<Diploma> = (fun (Diploma diploma) -> Encode.string diploma)
