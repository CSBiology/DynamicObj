module DynamicObj.Tests.GetHashCode

open Fable.Pyxpecto
open DynamicObj
open TestUtils


let tests_GetHashCode = testList "GetHashCode" [
    testCase "Same Object" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        Expect.equal (a.GetHashCode()) (a.GetHashCode()) "Values should be equal"

    testCase "Different Equal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 2)
        Expect.equal (a.GetHashCode()) (a2.GetHashCode()) "Values should be equal"

    testCase "Different Unequal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a.SetProperty("b", 3)
        Expect.notEqual (a.GetHashCode()) (a2.GetHashCode()) "Values should not be equal"

    testCase "nested DynamicObjs" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        b.SetProperty("c", 2)
        a.SetProperty("b", b)
        let a2 = DynamicObj()
        let b2 = DynamicObj()
        b2.SetProperty("c", 2)
        a2.SetProperty("b", b2)
        Expect.equal (a.GetHashCode()) (a2.GetHashCode()) "Values should be equal"

    testCase "null Value same key" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", null)
        let b = DynamicObj()
        b.SetProperty("b", null)
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Values should be equal"

    testCase "null Value different key" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", null)
        let b = DynamicObj()
        a.SetProperty("c", null)
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Values should not be equal"

]