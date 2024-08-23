module Rommulbad.Model.Validation

open System
open System.Text.RegularExpressions

/// Validate that the given string is not empty or indicate that the value is invalid by returning the provided value.
let nonEmpty invalid s = if String.IsNullOrWhiteSpace s then Error invalid else Ok s

/// Validate that the given string contains only alphanumeric characters or indicate otherwise by returning the provided
/// value.
let alphaNumeric invalid s = if String.forall Char.IsLetterOrDigit s then Ok s else Error invalid

/// Validate that the given string matches the provided regular expression or indicate otherwise by returning the
/// provided value.
let matches (re : Regex) invalid (s: string) = if re.IsMatch s then Ok s else Error invalid

let isNotNegative invalid (n: int) = 
    if n >= 0 then Ok n else Error invalid

let lessThanOrEqual invalid (max: int) (n: int) = 
    if n <= max then Ok n else Error invalid

let greaterThanOrEqual invalid (min: int) (n: int) = 
    if n >= min then Ok n else Error invalid