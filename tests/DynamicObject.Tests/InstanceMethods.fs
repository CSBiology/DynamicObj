module InstanceMethods.Tests

open System
open Fable.Pyxpecto
open DynamicObj

#if !FABLE_COMPILER
// instance method TryGetTypedValue is not Fable-compatible
let tests_TryGetTypedValue = testList "TryGetTypedValue" [
    
    testCase "typeof" <| fun _ -> 
        let a = typeof<int>
        Expect.equal a.Name "Int32" "Type should be Int32"

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a.TryGetTypedValue<int> "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = a.TryGetTypedValue<int> "a"
        Expect.equal b (Some 1) "Value should be 1"

    testCase "Incorrect Int" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = a.TryGetTypedValue<int> "a"
        Expect.isNone b "Value should not be an int"

    testCase "Correct String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = a.TryGetTypedValue<string> "a"
        Expect.equal b (Some "1") "Value should be '1'"

    testCase "Incorrect String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = a.TryGetTypedValue<string> "a"
        Expect.isNone b "Value should not be a string"

    testCase "Correct List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = a.TryGetTypedValue<int list> "a"
        Expect.equal b (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    ptestCase "Incorrect List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = a.TryGetTypedValue<string list> "a"
        Expect.isNone b "Value should not be a string list"

    testCase "Correct DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetValue("a", b)
        let c = a.TryGetTypedValue<DynamicObj> "a"
        Expect.equal c (Some b) "Value should be a DynamicObj"

    testCase "Incorrect DynamicObj" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = a.TryGetTypedValue<DynamicObj> "a"
        Expect.isNone b "Value should not be a DynamicObj"
]
#endif

let tests_SetValue = testList "SetValue" [

    testCase "Same String" <| fun _ ->
        let a = DynamicObj ()
        a.SetValue("aaa", 5)
        let b = DynamicObj ()
        b.SetValue("aaa", 5)
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = DynamicObj ()
        a.SetValue("aaa", 1212)
        let b = DynamicObj ()
        b.SetValue("aaa", 5)
        Expect.notEqual a b "Values should not be equal"

    testCase "String only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        b.SetValue("aaa", 5)

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"

    testCase "Same lists different keys" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!1", [1; 2; 3])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3; 4; 34])
        Expect.notEqual (a'.GetHashCode()) (b'.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        b.SetValue("aaa", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        b.SetValue("aaa1", b')
        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_Remove = testList "Remove" [
  
    testCase "Remove" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")

        a.Remove "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")
        b.SetValue("quack!", "hello")

        a.Remove "quecky!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")
        b.SetValue("quack!", "hello")

        a.Remove "quack!" |> ignore

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        a.Remove "quack!" |> ignore
        b.SetValue("aaa", b')

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Nested Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        a'.Remove "quack!" |> ignore
        b.SetValue("aaa", b')

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove on both" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        a.Remove "quack!" |> ignore
        b.SetValue("aaa", b')
        b.Remove "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]

let tests_GetPropertyHelpers = testList "GetPropertyHelpers" [
    testCase "GetPropertyHelpers" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        a.SetValue("b", 2)
        let properties = a.GetPropertyHelpers(true)
        let names = properties |> Seq.map (fun p -> p.Name)
        Expect.equal (Seq.toList names) ["a"; "b"] "Should have all properties"
]

let tests_copyDynamicProperties = testList "CopyDynamicProperties" [
    testCase "NewObject" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        a.SetValue("b", 2)
        let b = a.CopyDynamicProperties()
        Expect.equal a b "Values should be equal"
    testCase "ExistingObject" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        a.SetValue("b", 2)
        let b = DynamicObj()
        b.SetValue("c", 3)
        a.CopyDynamicPropertiesTo(b)
        Expect.equal (b.GetValue("a")) 1 "Value a should be copied"
        Expect.equal (b.GetValue("b")) 2 "Value b should be copied"
        Expect.equal (b.GetValue("c")) 3 "Value c should be unaffected"
    testCase "NoOverwrite throws" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = DynamicObj()
        b.SetValue("a", 3)
        let f = fun () -> a.CopyDynamicPropertiesTo(b)
        Expect.throws f "Should throw because property exists"
    testCase "Overwrite" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = DynamicObj()
        b.SetValue("a", 3)
        Expect.notEqual a b "Values should not be equal before copying"
        a.CopyDynamicPropertiesTo(b, true)
        Expect.equal a b "Values should be equal"
]

let main = testList "Instance Methods" [

    #if !FABLE_COMPILER
    // instance method TryGetTypedValue is not Fable-compatible
    tests_TryGetTypedValue
    #endif

    tests_SetValue
    tests_Remove
    tests_copyDynamicProperties
]