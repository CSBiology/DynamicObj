module DynamicObj.Tests.Equals

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_Equals = testList "Equals" [
    testCase "Same Object" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        Expect.isTrue (a.Equals(a)) "Values should be equal"

    testCase "Different Equal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 2)
        Expect.isTrue (a.Equals(a2)) "Values should be equal"

    testCase "Different Unequal Objects" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("b", 2)
        let a2 = DynamicObj()
        a2.SetProperty("b", 3)
        Expect.isFalse (a.Equals(a2)) "Values should not be equal"

    testCase "nested DynamicObjs" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        b.SetProperty("c", 2)
        a.SetProperty("b", b)
        let a2 = DynamicObj()
        let b2 = DynamicObj()
        b2.SetProperty("c", 2)
        a2.SetProperty("b", b2)
        Expect.isTrue (a.Equals(a2)) "Values should be equal"

]