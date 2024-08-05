module ReflectionUtils.Tests

open System
open Fable.Pyxpecto
open DynamicObj


let tests_baseObject = testList "Dynamic Set" [
    testCase "Same String" <| fun _ ->
        //ReflectionUtils.tryGetPropertyValue
        Expect.isTrue true "daw"
]

let main = testList "ReflectionUtils" [
    tests_baseObject
]