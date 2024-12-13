module DynamicObj.Tests.TryGetTypedPropertyValue

open Fable.Pyxpecto
open DynamicObj
open TestUtils

#if !FABLE_COMPILER
// instance method TryGetTypedPropertyValue is not Fable-compatible
let tests_TryGetTypedPropertyValue = testList "TryGetTypedPropertyValue" [
    
    testCase "typeof" <| fun _ -> 
        let a = typeof<int>
        Expect.equal a.Name "Int32" "Type should be Int32"

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.equal b (Some 1) "Value should be 1"

    testCase "Incorrect Int" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetTypedPropertyValue<int> "a"
        Expect.isNone b "Value should not be an int"

    testCase "Correct String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", "1")
        let b = a.TryGetTypedPropertyValue<string> "a"
        Expect.equal b (Some "1") "Value should be '1'"

    testCase "Incorrect String" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<string> "a"
        Expect.isNone b "Value should not be a string"

    testCase "Correct List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetTypedPropertyValue<int list> "a"
        Expect.equal b (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Incorrect List" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", [1; 2; 3])
        let b = a.TryGetTypedPropertyValue<string list> "a"
        Expect.isNone b "Value should not be a string list"

    testCase "Correct DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetProperty("a", b)
        let c = a.TryGetTypedPropertyValue<DynamicObj> "a"
        Expect.equal c (Some b) "Value should be a DynamicObj"

    testCase "Incorrect DynamicObj" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = a.TryGetTypedPropertyValue<DynamicObj> "a"
        Expect.isNone b "Value should not be a DynamicObj"
]
#endif