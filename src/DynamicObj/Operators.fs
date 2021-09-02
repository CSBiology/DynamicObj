module DynamicObj.Operators



/// Returns an instance with:
/// 1. this property added if it wasn't present
/// 2. this property updated otherwise
let (++) object (name, newValue) = ImmutableDynamicObj.add name newValue object


/// Returns an instance:
/// 1. the same if there was no requested property
/// 2. without the requested property if there was
let (--) object name = ImmutableDynamicObj.remove name object


/// Acts as (++) if the value is Some,
/// returns the same object otherwise
let (++?) object (name, newValue) =
    match newValue with
    | Some(value) -> object ++ (name, value)
    | None -> object

/// Acts as (++?) but maps the valid value 
/// through the last argument
let (++??) object (name, newValue, f) =
    match newValue with
    | Some(value) -> object ++ (name, f value)
    | None -> object