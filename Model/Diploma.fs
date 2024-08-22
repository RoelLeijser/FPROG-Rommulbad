module Rommulbad.Model.Diploma

open Thoth.Json.Net

type Diploma =
    private
    | A
    | B
    | C
    | NoDiploma

let (|Diploma|) =
    function
    | A -> "A"
    | B -> "B"
    | C -> "C"
    | NoDiploma -> ""


module Diploma =
    let make rawDiploma =
        match rawDiploma with
        | "A" -> A
        | "B" -> B
        | "C" -> C
        | _ -> NoDiploma

    let shallowOk (diploma: Diploma) =
        match diploma with
        | A -> true
        | _ -> false

    let minMinutes (diploma: Diploma) =
        match diploma with
        | A -> 1
        | B -> 10
        | C -> 15
        | NoDiploma -> 0

    let totalMinutes (diploma: Diploma) =
        match diploma with
        | A -> 120
        | B -> 150
        | C -> 180
        | NoDiploma -> 0

    let encode: Encoder<Diploma> = (fun (Diploma diploma) -> Encode.string diploma)

    let decode: Decoder<Diploma> =
        Decode.string |> Decode.andThen (fun diploma -> Decode.succeed (make diploma))
