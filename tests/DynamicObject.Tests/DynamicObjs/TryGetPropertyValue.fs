module DynamicObj.Tests.TryGetPropertyValue

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_TryGetPropertyValue = testList "TryGetPropertyValue" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetPropertyValue "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box 1)) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int>) (Some 1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box "1")) "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<string>) (Some "1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b) (Some (box [1; 2; 3])) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetPropertyValue "a"
        Expect.equal (b |> Option.map unbox<int list>) (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetPropertyValue "a"
        Expect.equal (c) (Some (box b)) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetPropertyValue "a"
        Expect.equal (c |> Option.map unbox<DynamicObj>) (Some b) "Value should be a DynamicObj"

]