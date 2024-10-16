module DynamicObj.Tests

open System
open Fable.Pyxpecto
open DynamicObj

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

let tests_TryGetDynamicPropertyHelper = testList "TryGetDynamicPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetDynamicPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Existing dynamic property" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b =  Expect.wantSome (a.TryGetDynamicPropertyHelper("a")) "Value should exist"
        Expect.isFalse b.IsStatic "Properties should be static"
        Expect.isTrue b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    testCase "static property not retrieved as dynamic" <| fun _ -> 
        let a = DynamicObj()
        Expect.isNone (a.TryGetDynamicPropertyHelper("Properties")) "static property should not be retrieved via TryGetDynamicPropertyInfo"
]

let tests_TryGetPropertyHelper = testList "TryGetPropertyHelper" [
    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetPropertyHelper("a")
        Expect.isNone b "Value should not exist"

    testCase "Existing dynamic property" <| fun _ -> 
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b =  Expect.wantSome (a.TryGetPropertyHelper("a")) "Value should exist"
        Expect.isFalse b.IsStatic "Properties should be static"
        Expect.isTrue b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"

    testCase "Existing static property" <| fun _ -> 
        let a = DynamicObj()
        let b =  Expect.wantSome (a.TryGetPropertyHelper("Properties")) "Value should exist"
        Expect.isTrue b.IsStatic "Properties should be static"
        Expect.isFalse b.IsDynamic "Properties should not be dynamic"
        Expect.isTrue b.IsMutable "Properties should be mutable"
        Expect.isFalse b.IsImmutable "Properties should not be immutable"
]

let tests_SetProperty = testList "SetProperty" [

    //TODO: static property accession!

    testCase "Same String" <| fun _ ->
        let a = DynamicObj ()
        a.SetProperty("aaa", 5)
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = DynamicObj ()
        a.SetProperty("aaa", 1212)
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)
        Expect.notEqual a b "Values should not be equal"

    testCase "String only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        b.SetProperty("aaa", 5)

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"

    testCase "Same lists different keys" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!1", [1; 2; 3])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3; 4; 34])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        b.SetProperty("aaa", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        b.SetProperty("aaa1", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_RemoveProperty = testList "RemoveProperty" [
  
    //TODO: static property removal!

    testCase "Remove" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")

        a.RemoveProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a.RemoveProperty "quecky!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetProperty("quack!", "hello")
        b.SetProperty("quack!", "hello")

        a.RemoveProperty "quack!" |> ignore

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a.RemoveProperty "quack!" |> ignore
        b.SetProperty("aaa", b')

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Nested Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a'.RemoveProperty "quack!" |> ignore
        b.SetProperty("aaa", b')

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove on both" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetProperty("quack!", [1; 2; 3])
        b'.SetProperty("quack!", [1; 2; 3])

        a.SetProperty("aaa", a')
        a.RemoveProperty "quack!" |> ignore
        b.SetProperty("aaa", b')
        b.RemoveProperty "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]

let tests_GetPropertyHelpers = testList "GetPropertyHelpers" [
    testCase "GetPropertyHelpers" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetPropertyHelpers(true)
        let names = properties |> Seq.map (fun p -> p.Name)
        Expect.equal (Seq.toList names) ["a"; "b"] "Should have all properties"
]

let tests_GetProperties = testList "GetProperties" [
    testCase "GetProperties" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let properties = a.GetProperties(true) |> List.ofSeq
        let expected = [
            System.Collections.Generic.KeyValuePair("a", box 1)
            System.Collections.Generic.KeyValuePair("b", box 2)
        ]
        Expect.equal properties expected "Should have all properties"
]

let tests_CopyDynamicPropertiesTo = testList "CopyDynamicPropertiesTo" [
    testCase "ExistingObject" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let b = DynamicObj()
        b.SetProperty("c", 3)
        a.CopyDynamicPropertiesTo(b)
        Expect.equal (b.GetPropertyValue("a")) 1 "Value a should be copied"
        Expect.equal (b.GetPropertyValue("b")) 2 "Value b should be copied"
        Expect.equal (b.GetPropertyValue("c")) 3 "Value c should be unaffected"

    testCase "NoOverwrite throws" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynamicObj()
        b.SetProperty("a", 3)
        let f = fun () -> a.CopyDynamicPropertiesTo(b)
        Expect.throws f "Should throw because property exists"

    testCase "Overwrite" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        let b = DynamicObj()
        b.SetProperty("a", 3)
        Expect.notEqual a b "Values should not be equal before copying"
        a.CopyDynamicPropertiesTo(b, true)
        Expect.equal a b "Values should be equal"
]

let tests_CopyDynamicProperties = testList "CopyDynamicProperties" [
    testCase "NewObject" <| fun _ ->
        let a = DynamicObj()
        a.SetProperty("a", 1)
        a.SetProperty("b", 2)
        let b = a.CopyDynamicProperties()
        Expect.equal a b "Values should be equal"
]

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

let main = testList "DynamicObj (Class)" [
    tests_TryGetPropertyValue
    tests_GetPropertyValue

    #if !FABLE_COMPILER
    // instance method TryGetTypedValue is not Fable-compatible
    tests_TryGetTypedPropertyValue
    #endif

    tests_TryGetStaticPropertyHelper
    tests_TryGetDynamicPropertyHelper
    tests_TryGetPropertyHelper
    tests_SetProperty
    tests_RemoveProperty
    tests_GetPropertyHelpers
    tests_GetProperties
    tests_CopyDynamicPropertiesTo
    tests_CopyDynamicProperties
    tests_Equals
    tests_GetHashCode
]