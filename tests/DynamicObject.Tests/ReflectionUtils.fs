module ReflectionUtils.Tests

open System
open Fable.Pyxpecto
open DynamicObj


let tests_baseObject = testList "Dynamic Set" [
    testCase "Same String" <| fun _ ->
        Expect.isTrue false "ehllo"
]

let main = testList "DynamicObj" [
    tests_baseObject
]