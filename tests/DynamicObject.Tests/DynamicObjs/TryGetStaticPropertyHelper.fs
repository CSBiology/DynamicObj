module DynamicObj.Tests.TryGetStaticPropertyHelper

open Fable.Pyxpecto
open DynamicObj
open TestUtils

let tests_TryGetStaticPropertyHelper = testList "TryGetStaticPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetStaticPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Properties dictionary is static property" <| fun _ -> 
        let a = DynamicObj()
        let b =  Expect.wantSome (a.TryGetStaticPropertyHelper("Properties")) "Value should exist"
        Expect.isTrue b.IsStatic "Properties should be static"
        Expect.isFalse b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    testCase "dynamic property not retrieved as static" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        Expect.isNone (a.TryGetStaticPropertyHelper("a")) "dynamic property should not be retrieved via TryGetStaticPropertyInfo"
]