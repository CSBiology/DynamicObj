module DynamicObj.Tests.GetPropertyValue

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_GetPropertyValue = testList "GetPropertyValue" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        Expect.throws (fun () -> a.GetPropertyValue("b") |> ignore) "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box 1) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<int>) (1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box "1") "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<string>) ("1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.GetPropertyValue "a"
        Expect.equal (b) (box [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.GetPropertyValue "a"
        Expect.equal (b |> unbox<int list>) ([1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.GetPropertyValue "a"
        Expect.equal (c) (box b) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.GetPropertyValue "a"
        Expect.equal (c |> unbox<DynamicObj>) (b) "Value should be a DynamicObj"

]