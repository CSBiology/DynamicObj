#load "ReflectionUtils.fs"
#load "ImmutableDynamicObj.fs"
#load "DynamicObj.fs"
#load "DynObj.fs"

open DynamicObj


let foo = DynamicObj()
foo?bar <- [1;2;3;4]

(DynObj.print foo)

let fooIDO = 
    ImmutableDynamicObj()
    |> ImmutableDynamicObj.add "foo" "bar" 
    |> ImmutableDynamicObj.add "inner" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerfoo" "innerbar" )
    |> ImmutableDynamicObj.add "inner2" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerinner" (ImmutableDynamicObj() |> ImmutableDynamicObj.add "innerinnerfoo" "innerinnerbar" ))
    
printfn "%s" (fooIDO |> ImmutableDynamicObj.format)
