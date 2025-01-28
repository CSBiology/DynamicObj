module HashUtils.Tests

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core

let int1 = box 1
let int2 = box 2
let int3 = box 3
let int4 = box 4

let intDict1 = 
    let d = System.Collections.Generic.Dictionary<obj, obj>()
    d.Add(int1, int2)
    d.Add(int3, int4)
    d

let intDict1' = 
    let d = System.Collections.Generic.Dictionary<obj, obj>()
    d.Add(int1, int2)
    d.Add(int3, int4)
    d

let intDict2 =
    let d = System.Collections.Generic.Dictionary<obj, obj>()
    d.Add(int1, int4)
    d.Add(int3, int2)
    d

let intDict3 =
    let d = System.Collections.Generic.Dictionary<obj, obj>()
    d.Add(int2, int1)
    d.Add(int4, int3)
    d

let intDict4 =
    let d = System.Collections.Generic.Dictionary<obj, obj>()
    d.Add(int1, int3)
    d.Add(int2, int4)
    d

let intList1 = [int1;int2;int3;int4]
let intList1' = [int1;int2;int3;int4]
let intList2 = [int1;int4;int3;int2]

let nestedList1 = [intList1;intList2]
let nestedList1' = [intList1';intList2]
let nestedList2 = [intList2;intList1]

let intArray1 = [|int1;int2;int3;int4|]
let intArray1' = [|int1;int2;int3;int4|]
let intArray2 = [|int1;int4;int3;int2|]

let intSeq1 = seq { yield int1; yield int2; yield int3; yield int4 }
let intSeq1' = seq { yield int1; yield int2; yield int3; yield int4 }
let intSeq2 = seq { yield int1; yield int4; yield int3; yield int2 }

let resizeArray1 = ResizeArray [int1;int2;int3;int4]
let resizeArray1' = ResizeArray [int1;int2;int3;int4]
let resizeArray2 = ResizeArray [int1;int4;int3;int2]


let dynamicObjectWithInt1 =

    let d = DynamicObj()
    d.SetProperty("a", int1)
    d.SetProperty("b", int2)
    d

let dynamicObjectWithInt1DiffKey =
    let d = DynamicObj()
    d.SetProperty("a", int1)
    d.SetProperty("c", int2)
    d

let dynamicObjectWithInt1' =  
    let d = DynamicObj()
    d.SetProperty("a", int1)
    d.SetProperty("b", int2)
    d

let dynamicObjectWithInt2 =
    let d = DynamicObj()
    d.SetProperty("a", int2)
    d.SetProperty("b", int1)
    d

let dynamicObjectWithDict1 =
    let d = DynamicObj()
    d.SetProperty("a", intDict1)
    d.SetProperty("b", intDict2)
    d

let dynamicObjectWithDict1' =
    let d = DynamicObj()
    d.SetProperty("a", intDict1)
    d.SetProperty("b", intDict2)
    d

let dynamicObjectWithDict2 =
    let d = DynamicObj()
    d.SetProperty("a", intDict2)
    d.SetProperty("b", intDict1)
    d

let dynamicObjectWithDynamicObject1 =
    let d = DynamicObj()
    d.SetProperty("a", dynamicObjectWithInt1)
    d.SetProperty("b", dynamicObjectWithInt2)
    d

let dynamicObjectWithDynamicObject1' =
    let d = DynamicObj()
    d.SetProperty("a", dynamicObjectWithInt1)
    d.SetProperty("b", dynamicObjectWithInt2)
    d

let dynamicObjectWithDynamicObject2 =
    let d = DynamicObj()
    d.SetProperty("a", dynamicObjectWithInt2)
    d.SetProperty("b", dynamicObjectWithDict1)
    d


let tests_Dictionary =
    testList "Dictionary" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->               
                Expect.equal (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict1) "Same Dictionary should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict1') "Structurally equal Dictionary should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict2) "Different Dictionary should return different Hash (1vs2)"
            testCase "1v3" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict3) "Different Dictionary should return different Hash (1vs3)"
            testCase "1v4" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict4) "Different Dictionary should return different Hash (1vs4)"
            testCase "2v3" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intDict2) (HashUtils.deepHash intDict3) "Different Dictionary should return different Hash (2vs3)"
            testCase "2v4" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intDict2) (HashUtils.deepHash intDict4) "Different Dictionary should return different Hash (2vs4)"
                    
        ]
    ]

let tests_Lists = 
    testList "Lists" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->               
                Expect.equal (HashUtils.deepHash intList1) (HashUtils.deepHash intList1) "Same List should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash intList1) (HashUtils.deepHash intList1') "Structurally equal List should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intList1) (HashUtils.deepHash intList2) "Different List should return different Hash"
        ]
        testList "Shuffled Nested" [
            testCase "1v1" <| fun _ ->               
                Expect.equal (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList1) "Same Nested List should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList1') "Structurally equal Nested List should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList2) "Different Nested List should return different Hash"
       
        ]
    ]

let tests_Array = 
    testList "Array" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->               
                Expect.equal (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray1) "Same Array should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray1') "Structurally equal Array should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray2) "Different Array should return different Hash"
        ]
    ]

let tests_Seq = 
    testList "Seq" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->               
                Expect.equal (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq1) "Same Seq should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq1') "Structurally equal Seq should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq2) "Different Seq should return different Hash"
        ]
    ]

let tests_ResizeArray = 
    testList "ResizeArray" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->
                
                Expect.equal (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray1) "Same ResizeArray should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray1') "Structurally equal ResizeArray should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray2) "Different ResizeArray should return different Hash"
        ]
    ]


let tests_DynamicObject = 
    testList "DynamicObj" [
        testList "Shuffled Int" [
            testCase "1v1" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithInt1) (HashUtils.deepHash dynamicObjectWithInt1) "Same DynamicObject should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithInt1) (HashUtils.deepHash dynamicObjectWithInt1') "Structurally equal DynamicObject should return consistent Hash"
            testCase "1v1DiffKey" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithInt1) (HashUtils.deepHash dynamicObjectWithInt1DiffKey) "Different DynamicObject should return different Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithInt1) (HashUtils.deepHash dynamicObjectWithInt2) "Different DynamicObject should return different Hash"
        ]
        testList "Shuffled Dict" [
            testCase "1v1" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithDict1) (HashUtils.deepHash dynamicObjectWithDict1) "Same DynamicObject should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithDict1) (HashUtils.deepHash dynamicObjectWithDict1') "Structurally equal DynamicObject should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithDict1) (HashUtils.deepHash dynamicObjectWithDict2) "Different DynamicObject should return different Hash"
        ]
        testList "Shuffled DynamicObject" [
            testCase "1v1" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithDynamicObject1) (HashUtils.deepHash dynamicObjectWithDynamicObject1) "Same DynamicObject should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash dynamicObjectWithDynamicObject1) (HashUtils.deepHash dynamicObjectWithDynamicObject1') "Structurally equal DynamicObject should return consistent Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithDynamicObject1) (HashUtils.deepHash dynamicObjectWithDynamicObject2) "Different DynamicObject should return different Hash"
        ]
        testList "Shuffled Int AsOption" [
            testCase "1v1" <| fun _ ->
                Expect.equal (HashUtils.deepHash (Some dynamicObjectWithInt1)) (HashUtils.deepHash (Some dynamicObjectWithInt1)) "Same DynamicObject should return consistent Hash"
            testCase "1v1'" <| fun _ ->
                Expect.equal (HashUtils.deepHash (Some dynamicObjectWithInt1)) (HashUtils.deepHash (Some dynamicObjectWithInt1')) "Structurally equal DynamicObject should return consistent Hash"
            testCase "1v1DiffKey" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash (Some dynamicObjectWithInt1)) (HashUtils.deepHash (Some dynamicObjectWithInt1DiffKey)) "Different DynamicObject should return different Hash"
            testCase "1v2" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash (Some dynamicObjectWithInt1)) (HashUtils.deepHash (Some dynamicObjectWithInt2)) "Different DynamicObject should return different Hash"
            testCase "1 v None" <| fun _ ->
                Expect.notEqual (HashUtils.deepHash (Some dynamicObjectWithInt1)) (HashUtils.deepHash None) "Different DynamicObject should return different Hash"
            
        ]
        testList "Mixed" [
            testCase "Int vs Dict" <| fun _ ->               
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithInt1) (HashUtils.deepHash dynamicObjectWithDict1) "Int vs Dict with same values should return different Hash"
            testCase "Dict vs DynObj" <| fun _ ->               
                Expect.notEqual (HashUtils.deepHash dynamicObjectWithDict1) (HashUtils.deepHash dynamicObjectWithDynamicObject1) "Dict vs DynObj with same values should return different Hash"           
        ]
    ]


let tests_Mixed = 
    testList "Mixed" [
        testCase "Int vs Dict" <| fun _ ->               
            Expect.notEqual (HashUtils.deepHash int1) (HashUtils.deepHash intDict1) "Int vs Dict with same values should return different Hash"
        testCase "List vs Dict" <| fun _ ->               
            Expect.notEqual (HashUtils.deepHash intList1) (HashUtils.deepHash intDict1) "List vs Dict with same values should return different Hash"
    ]




let main = testList "DeepHash" [
    tests_Dictionary
    tests_Lists
    tests_Array
    tests_Seq
    tests_ResizeArray
    tests_DynamicObject
    tests_Mixed
]