module DynamicObj.Tests

open System
open Fable.Pyxpecto
open DynamicObj

let tests_tryGetTypedValue = testList "TryGetTypedValue" [
    
    testCase "typeof" <| fun _ -> 
        let a = typeof<int>
        Expect.equal a.Name "Int32" "Type should be Int32"

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = DynObj.tryGetTypedValue<int> "a" a
        Expect.isNone b "Value should not exist"

    testCase "Correct Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = DynObj.tryGetTypedValue<int> "a" a
        Expect.equal b (Some 1) "Value should be 1"

    testCase "Incorrect Int" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = DynObj.tryGetTypedValue<int> "a" a
        Expect.isNone b "Value should not be an int"

    testCase "Correct String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = DynObj.tryGetTypedValue<string> "a" a
        Expect.equal b (Some "1") "Value should be '1'"

    testCase "Incorrect String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = DynObj.tryGetTypedValue<string> "a" a
        Expect.isNone b "Value should not be a string"

    testCase "Correct List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = DynObj.tryGetTypedValue<int list> "a" a
        Expect.equal b (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    ptestCase "Incorrect List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = DynObj.tryGetTypedValue<string list> "a" a
        Expect.isNone b "Value should not be a string list"

    testCase "Correct DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetValue("a", b)
        let c = DynObj.tryGetTypedValue<DynamicObj> "a" a
        Expect.equal c (Some b) "Value should be a DynamicObj"

    testCase "Incorrect DynamicObj" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = DynObj.tryGetTypedValue<DynamicObj> "a" a
        Expect.isNone b "Value should not be a DynamicObj"
]
    

let tests_set = testList "Set" [

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

let tests_remove = testList "Remove" [
  
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


let tests_formatString = testList "FormatString" [

    testCase "Format string 1" <| fun _ ->
        let foo = DynamicObj()
        let list = [1;2;3;4]
        foo.SetValue("bar", list)
        let expected = $"?bar: {list}"
        Expect.equal (foo |> DynObj.format) expected "Format string 1 failed"

    testCase "Format string 2" <| fun _ ->
        let foo = DynamicObj()
        let corgi = "corgi"
        foo.SetValue("corgi", corgi)
        let inner = DynamicObj()
        let baz = "baz"
        inner.SetValue("bar", baz)
        foo.SetValue("foo", inner)
        let expected = $"""?corgi: {corgi}{Environment.NewLine}?foo:{Environment.NewLine}    ?bar: {baz}"""
        Expect.equal (foo |> DynObj.format) expected "Format string 2 failed"

]


let tests_combine = testList "Combine" [

    testCase "Combine flat DOs" <| fun _ ->
        let target = DynamicObj()

        target.SetValue("target-unique", [42])
        target.SetValue("will-be-overridden", "WAS_NOT_OVERRIDDEN!")

        let source = DynamicObj()

        source.SetValue("source-unique", [42; 32])
        source.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")

        let combined = DynObj.combine target source

        let expected = DynamicObj()

        expected.SetValue("target-unique", [42])
        expected.SetValue("source-unique", [42; 32])
        expected.SetValue("will-be-overridden", "WAS_OVERRIDDEN =)")

        Expect.equal expected combined "Combine flat DOs failed"

    testCase "Combine nested DOs" <| fun _ ->
        let target = DynamicObj()

        target.SetValue("target-unique", 1337)
        target.SetValue("will-be-overridden", -42)
        let something2BeCombined = DynamicObj()
        something2BeCombined.SetValue("inner","I Am")
        let something2BeOverriden = DynamicObj()
        something2BeOverriden.SetValue("inner","NOT_OVERRIDDEN")
        target.SetValue("nested-will-be-combined", something2BeCombined)
        target.SetValue("nested-will-be-overridden", something2BeOverriden)
    
        let source = DynamicObj()

        source.SetValue("source-unique", 69)
        source.SetValue("will-be-overridden", "WAS_OVERRIDDEN")
        let alsoSomething2BeCombined = DynamicObj()
        alsoSomething2BeCombined.SetValue("inner_combined","Complete")
        source.SetValue("nested-will-be-combined", alsoSomething2BeCombined)
        source.SetValue("nested-will-be-overridden", "WAS_OVERRIDDEN")
    
        let combined = DynObj.combine target source
    
        let expected = DynamicObj()

        expected.SetValue("source-unique", 69)
        expected.SetValue("target-unique", 1337)
        expected.SetValue("will-be-overridden", "WAS_OVERRIDDEN")
        expected.SetValue("nested-will-be-overridden", "WAS_OVERRIDDEN")
        expected.SetValue("nested-will-be-combined", 
            let inner = DynamicObj()
            inner.SetValue("inner","I Am")
            inner.SetValue("inner_combined","Complete")
            inner
            )

        Expect.equal expected combined "Combine nested DOs failed"
]

let tests_print = testList "Print" [

    testCase "Test Print For Issue 14" <| fun _ ->
        let outer = DynamicObj()
        let inner = DynamicObj()
        inner.SetValue("Level", "Information")
        inner.SetValue("MessageTemplate","{Method} Request at {Path}")
        outer.SetValue("serilog", inner)

        let print =
            try 
                outer |> DynObj.print
                true
            with
                | e -> false

        Expect.isTrue print "Print failed for issue 14"
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

let main = testList "DynamicObj" [
    tests_tryGetTypedValue
    tests_set
    tests_remove
    tests_formatString
    tests_combine
    tests_copyDynamicProperties
]