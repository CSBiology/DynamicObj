module Main.Tests

open Fable.Pyxpecto

let all = testSequenced <| testList "DynamicObj" [
    ReflectionUtils.Tests.main
    DynamicObj.Tests.main
    Inheritance.Tests.main
    Interface.Tests.main
]

[<EntryPoint>]
let main argv = Pyxpecto.runTests [||] all