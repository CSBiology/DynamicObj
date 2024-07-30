module DynamicObj.Tests

open System
open Fable.Pyxpecto
open DynamicObj


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

    testCase "Same list" <| fun _ ->
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
  
    testCase "Nested Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a'.SetValue("quack!", [1; 2; 3])
        b'.SetValue("quack!", [1; 2; 3])

        a.SetValue("aaa", a')
        a.Remove "quack!" |> ignore
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
        foo.SetValue("bar", [1;2;3;4])
        let expected = "?bar: [1; 2; 3; ... ]"
        Expect.equal expected (foo |> DynObj.format) "Format string 1 failed"

    testCase "Format string 2" <| fun _ ->
        let foo = DynamicObj()
        foo.SetValue("corgi", "corgi")
        let inner = DynamicObj()
        inner.SetValue("bar", "baz")
        foo.SetValue("foo", inner)
        let expected = $"""?corgi: corgi{Environment.NewLine}?foo:{Environment.NewLine}    ?bar: baz"""
        Expect.equal expected (foo |> DynObj.format) "Format string 2 failed"

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

let main = testList "DynamicObj" [
    tests_set
    tests_remove
    tests_formatString
    tests_combine
]