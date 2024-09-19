module Main.Tests

open Fable.Pyxpecto

let all = testSequenced <| testList "DynamicObj" [
    ReflectionUtils.Tests.main
    InstanceMethods.Tests.main
    DynObjStaticMethods.Tests.main
    Inheritance.Tests.main
    Interface.Tests.main
]

[<EntryPoint>]
let main argv = Pyxpecto.runTests [||] all