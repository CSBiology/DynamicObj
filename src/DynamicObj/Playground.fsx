#r "nuget: Newtonsoft.Json"
#load "./ReflectionUtils.fs"
#load "./ImmutableDynamicObj.fs"
#load "./DynamicObj.fs"
#load "./DynObj.fs"
#load "./Operators.fs"

open DynamicObj
open DynamicObj.Operators

let target = DynamicObj()

target.SetValue("target-unique", [42])
target.SetValue("will-be-overridden", "WAS_NOT_OVERRIDDEN!")

let source = DynamicObj()

source.SetValue("source-unique", [|42|])
source.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")

let combined = DynObj.combine target source

let expected = DynamicObj()

expected.SetValue("target-unique", [42])
expected.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")
expected.SetValue("source-unique", [|42|])

combined = expected

combined |> DynObj.print
expected |> DynObj.print


let foo = DynamicObj()
foo?bar <- [1;2;3;4]

(DynObj.print foo)

let fooIDO = 
    ImmutableDynamicObj()
    |> ImmutableDynamicObj.add "foo" "bar" 
    |> ImmutableDynamicObj.add "inner" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerfoo" "innerbar" )
    |> ImmutableDynamicObj.add "inner2" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerinner" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerinnerfoo" "innerinnerbar" ))
    
printfn "%s" (fooIDO |> ImmutableDynamicObj.format)

let o2 =
    ImmutableDynamicObj.empty
    ++ ("aaa", 5)
    ++ ("ohno", 10)
    ++ ("quack", "tt")
    ++ ("hh", [1; 2; 3])
    
let o =
    ImmutableDynamicObj.empty
    ++ ("aaa", 5)
    ++ ("ohno", 10)
    ++ ("quack", "tt")
    ++ ("hh", [1; 2; 3])
    ++ ("inner", o2)

open Newtonsoft.Json

let actual = JsonConvert.SerializeObject o

printfn "%s" actual