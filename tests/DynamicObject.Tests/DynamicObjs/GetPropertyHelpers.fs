module DynamicObj.Tests.GetPropertyHelpers

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_GetPropertyHelpers = testList "GetPropertyHelpers" [
    testCase "GetPropertyHelpers" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetPropertyHelpers(true)
        let names = properties |> Seq.map (fun p -> p.Name)
        Expect.equal (Seq.toList names) ["a"; "b"] "Should have all properties"
]
