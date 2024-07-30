module Main.Tests

open Fable.Pyxpecto

let all = testSequenced <| testList "DynamicObj" [
    DynamicObj.Tests.main
    ReflectionUtils.Tests.main
]

[<EntryPoint>]
let main argv = Pyxpecto.runTests [||] all