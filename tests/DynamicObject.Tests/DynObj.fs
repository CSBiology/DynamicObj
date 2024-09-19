module DynObj.Tests

open System
open System.Collections.Generic
open Fable.Pyxpecto
open DynamicObj

let tests_ofDict = ptestList "ofDict" [
    testCase "Test ofDict" <| fun _ ->
        let d = Dictionary()
        d.Add("key1", box 1)
        d.Add("key2", box 2)
        let dyn = DynObj.ofDict d
        Expect.equal (dyn.GetValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetValue("key2")) 2 "Value should be 2"
]

let tests_ofSeq = ptestList "ofSeq" [
    testCase "Test ofDict" <| fun _ ->
        let d = 
            seq {
                "key1", box 1
                "key2", box 2
            }
        let dyn = DynObj.ofSeq d
        Expect.equal (dyn.GetValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetValue("key2")) 2 "Value should be 2"
]

let tests_ofList = ptestList "ofList" [
    testCase "Test ofDict" <| fun _ ->
        let d = [
            "key1", box 1
            "key2", box 2
        ]
        let dyn = DynObj.ofList d
        Expect.equal (dyn.GetValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetValue("key2")) 2 "Value should be 2"
]

let tests_ofArray = ptestList "ofArray" [
    testCase "Test ofDict" <| fun _ ->
        let d = [|
            "key1", box 1
            "key2", box 2
        |]
        let dyn = DynObj.ofArray d
        Expect.equal (dyn.GetValue("key1")) 1 "Value should be 1"
        Expect.equal (dyn.GetValue("key2")) 2 "Value should be 2"
]

let tests_combine = testList "combine" [

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

let tests_tryGetTypedValue = testList "tryGetTypedValue" [
    
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

let tests_setValue = testList "setValue" [

    testCase "Same String" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValue "aaa" 5
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 5
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValue "aaa" 1212
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 5
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "String only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"

    testCase "Same lists different keys" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setValue "quack!" [1; 2; 3]
        b |> DynObj.setValue "quack!1" [1; 2; 3]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setValue "quack!" [1; 2; 3]
        b |> DynObj.setValue "quack!" [1; 2; 3; 4; 34]
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()

        a' |> DynObj.setValue "quack!" [1; 2; 3]
        b' |> DynObj.setValue "quack!" [1; 2; 3]

        a |> DynObj.setValue "aaa" a'
        b |> DynObj.setValue "aaa" b'

        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        let a' = DynamicObj ()
        let b' = DynamicObj ()
        a' |> DynObj.setValue "quack!" [1; 2; 3]
        b' |> DynObj.setValue "quack!" [1; 2; 3]

        a |> DynObj.setValue "aaa" a'
        b |> DynObj.setValue "aaa1" b'

        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_withValue = testList "withValue" [

    testCase "Same String" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withValue "aaa" 5
            |> DynObj.withValue "aaaa" 6
        let b = 
            DynamicObj () 
            |> DynObj.withValue "aaa" 5
            |> DynObj.withValue "aaaa" 6
        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Different Strings" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withValue "111" 5
            |> DynObj.withValue "1111" 6
        let b = 
            DynamicObj () 
            |> DynObj.withValue "aaa" 5
            |> DynObj.withValue "aaaa" 6
        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "String different amounts" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withValue "aaa" 5
        let b = 
            DynamicObj () 
            |> DynObj.withValue "aaa" 5
            |> DynObj.withValue "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual b a "Values should not be equal (Reversed equality)"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   

    testCase "Same lists different keys" <| fun _ ->
        let a = 
            DynamicObj () 
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aa" [1; 2; 3]
        let b = 
            DynamicObj () 
            |> DynObj.withValue "aa" [1; 2; 3]
            |> DynObj.withValue "bbb" [1; 2; 3]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"   
   
    testCase "Different lists" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()
        a |> DynObj.setValue "quack!" [1; 2; 3]
        b |> DynObj.setValue "quack!" [1; 2; 3; 4; 34]

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should not be equal"

    testCase "Nested Same List Same String" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()

        let a = DynamicObj () |> DynObj.withValue "aaa" (a' |> DynObj.withValue "quack!" [1; 2; 3])
        let b = DynamicObj () |> DynObj.withValue "aaa" (b' |> DynObj.withValue "quack!" [1; 2; 3])

        Expect.equal a' b' "New Values should be equal"
        Expect.equal a b "Old Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Old Hash codes should be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"

    testCase "Nested Same List Different Strings" <| fun _ ->
        let a' = DynamicObj ()
        let b' = DynamicObj ()

        let a = DynamicObj () |> DynObj.withValue "aaa" (a' |> DynObj.withValue "quack!" [1; 2; 3])
        let b = DynamicObj () |> DynObj.withValue "aaa1" (b' |> DynObj.withValue "quack!" [1; 2; 3])

        Expect.equal a' b' "New Values should be equal"
        Expect.notEqual a b "Old Values should not be equal"
        Expect.equal (a'.GetHashCode()) (b'.GetHashCode()) "New Hash codes should be equal"
    ]

let tests_setValueOpt = testList "setValueOpt" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOpt "aaa" (Some 5)
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 5

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOpt "aaa" None
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOpt "aaa" (Some 5)
        a |> DynObj.setValueOpt "aaaa" None
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 5
        b |> DynObj.setValue "aaaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_withValueOpt = testList "withValueOpt" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj () |> DynObj.withValueOpt "aaa" (Some 5)
        let b = DynamicObj () |> DynObj.withValue "aaa" 5

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "None" <| fun _ ->
        let a = DynamicObj () |> DynObj.withValueOpt "aaa" None
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValueOpt "aaa" (Some 5)
            |> DynObj.withValueOpt "aaaa" None
        let b = 
            DynamicObj ()
            |> DynObj.withValue "aaa" 5
            |> DynObj.withValue "aaaa" 5

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 5 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_setValueOptBy = testList "setValueOptBy" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOptBy "aaa" (Some 5) (fun x -> x + 1)
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 6

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOptBy "aaa" None id
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = DynamicObj ()
        a |> DynObj.setValueOptBy "aaa" (Some 5) (fun x -> x + 1)
        a |> DynObj.setValueOptBy "aaaa" None id
        let b = DynamicObj ()
        b |> DynObj.setValue "aaa" 6
        b |> DynObj.setValue "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_withValueOptBy = testList "withValueOptBy" [
    testCase "Some" <| fun _ ->
        let a = DynamicObj () |> DynObj.withValueOptBy "aaa" (Some 5) (fun x -> x+1)
        let b = DynamicObj () |> DynObj.withValue "aaa" 6

        Expect.equal a b "Values should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "None" <| fun _ ->
        let a = DynamicObj () |> DynObj.withValueOptBy "aaa" None id 
        let b = DynamicObj ()

        Expect.equal a b "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get |> ignore) "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
    testCase "Some and None" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValueOptBy "aaa" (Some 5) (fun x -> x+1)
            |> DynObj.withValueOptBy "aaaa" None id
        let b = 
            DynamicObj ()
            |> DynObj.withValue "aaa" 6
            |> DynObj.withValue "aaaa" 6

        Expect.notEqual a b "Values should not be equal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"
        Expect.equal (a |> DynObj.tryGetTypedValue<int> "aaa" |> Option.get) 6 "Values should be equal"
        Expect.throws (fun _ -> a |> DynObj.tryGetTypedValue<int> "aaaa" |> Option.get |> ignore) "Values should not exist"
]

let tests_tryGetValue = testList "tryGetValue" [

    testCase "NonExisting" <| fun _ -> 
        let a = DynamicObj()
        let b = a |> DynObj.tryGetValue "a"
        Expect.isNone b "Value should not exist"

    testCase "Correct boxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b) (Some (box 1)) "Value should be 1"

    testCase "Correct unboxed Int" <| fun _ -> 
        let a = DynamicObj()
        a.SetValue("a", 1)
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b |> Option.map unbox<int>) (Some 1) "Value should be 1"

    testCase "Correct boxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b) (Some (box "1")) "Value should be '1'"

    testCase "Correct unboxed String" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", "1")
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b |> Option.map unbox<string>) (Some "1") "Value should be '1'"

    testCase "Correct boxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b) (Some (box [1; 2; 3])) "Value should be [1; 2; 3]"

    testCase "Correct unboxed List" <| fun _ ->
        let a = DynamicObj()
        a.SetValue("a", [1; 2; 3])
        let b = a |> DynObj.tryGetValue "a"
        Expect.equal (b |> Option.map unbox<int list>) (Some [1; 2; 3]) "Value should be [1; 2; 3]"

    testCase "Correct boxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetValue("a", b)
        let c = a |> DynObj.tryGetValue "a"
        Expect.equal (c) (Some (box b)) "Value should be a DynamicObj"

    testCase "Correct unboxed DynamicObj" <| fun _ ->
        let a = DynamicObj()
        let b = DynamicObj()
        a.SetValue("a", b)
        let c = a |> DynObj.tryGetValue "a"
        Expect.equal (c |> Option.map unbox<DynamicObj>) (Some b) "Value should be a DynamicObj"

]

let tests_remove = testList "remove" [
  
    testCase "Remove" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")

        a |> DynObj.remove "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")
        b.SetValue("quack!", "hello")

        a |> DynObj.remove "quecky!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = DynamicObj ()
        let b = DynamicObj ()

        a.SetValue("quack!", "hello")
        b.SetValue("quack!", "hello")

        a |> DynObj.remove "quack!" |> ignore

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
        a |> DynObj.remove "quack!" |> ignore
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
        a' |> DynObj.remove "quack!" |> ignore
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
        a |> DynObj.remove "quack!" |> ignore
        b.SetValue("aaa", b')
        b |> DynObj.remove "quack!" |> ignore

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

]


let tests_withoutProperty = testList "withoutProperty" [
  
    testCase "Remove" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "quack!" "hello"
            |> DynObj.withoutProperty "quack!"

        let b = DynamicObj ()


        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove Non-Existing" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "quack!" "hello"
            |> DynObj.withoutProperty "quecky!"

        let b = DynamicObj () |> DynObj.withValue "quack!" "hello"


        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Remove only on one" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "quack!" "hello"
            |> DynObj.withoutProperty "quack!"
        let b = DynamicObj () |> DynObj.withValue "quack!" "hello"

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove Non-Existing" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "quack!"

        let b =            
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())

        Expect.equal a b "Values should be equal"
        Expect.equal (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be equal"

    testCase "Nested Remove only on one" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

        let b =            
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())

        Expect.notEqual a b "Values should be unequal"
        Expect.notEqual (a.GetHashCode()) (b.GetHashCode()) "Hash codes should be unequal"

    testCase "Nested Remove on both" <| fun _ ->
        let a = 
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

        let b =            
            DynamicObj ()
            |> DynObj.withValue "a" [1; 2; 3]
            |> DynObj.withValue "aaa" (DynamicObj ())
            |> DynObj.withoutProperty "a"

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

let main = testList "DynObj (Module)" [
    tests_ofDict
    tests_ofSeq
    tests_ofList
    tests_ofArray
    tests_combine
    tests_tryGetTypedValue
    tests_setValue
    tests_withValue
    tests_setValueOpt
    tests_withValueOpt
    tests_setValueOptBy
    tests_withValueOptBy
    tests_tryGetValue
    tests_withoutProperty
    tests_formatString
    tests_print
]