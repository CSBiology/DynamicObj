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

let tests_Dictionary =
    testList "Dictionary" [
        testCase "Shuffled Int" <| fun _ ->               
            Expect.equal (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict2) "Same Dictionary should return consistent Hash"
            Expect.equal (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict1') "Strucutally equal Dictionary should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict2) "Different Dictionary should return different Hash (1vs2)"
            Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict3) "Different Dictionary should return different Hash (1vs3)"
            Expect.notEqual (HashUtils.deepHash intDict1) (HashUtils.deepHash intDict4) "Different Dictionary should return different Hash (1vs4)"
            Expect.notEqual (HashUtils.deepHash intDict2) (HashUtils.deepHash intDict3) "Different Dictionary should return different Hash (2vs3)"
            Expect.notEqual (HashUtils.deepHash intDict2) (HashUtils.deepHash intDict4) "Different Dictionary should return different Hash (2vs4)"
            Expect.notEqual (HashUtils.deepHash intDict3) (HashUtils.deepHash intDict4) "Different Dictionary should return different Hash (3vs4)"  
    ]

let tests_Lists = 
    testList "Lists" [
        testCase "Shuffled Int" <| fun _ ->               
            Expect.equal (HashUtils.deepHash intList1) (HashUtils.deepHash intList1) "Same List should return consistent Hash"
            Expect.equal (HashUtils.deepHash intList1) (HashUtils.deepHash intList1') "Strucutally equal List should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash intList1) (HashUtils.deepHash intList2) "Different List should return different Hash"
        testCase "Shuffled Nested" <| fun _ ->
            Expect.equal (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList1) "Same Nested List should return consistent Hash"
            Expect.equal (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList1') "Strucutally equal Nested List should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash nestedList1) (HashUtils.deepHash nestedList2) "Different Nested List should return different Hash"
    ]

let tests_Array =
    testList "Array" [
        testCase "Shuffled Int" <| fun _ ->               
            Expect.equal (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray1) "Same Array should return consistent Hash"
            Expect.equal (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray1') "Strucutally equal Array should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash intArray1) (HashUtils.deepHash intArray2) "Different Array should return different Hash"
    ]

let tests_Seq =
    testList "Seq" [
        testCase "Shuffled Int" <| fun _ ->               
            Expect.equal (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq1) "Same Seq should return consistent Hash"
            Expect.equal (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq1') "Strucutally equal Seq should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash intSeq1) (HashUtils.deepHash intSeq2) "Different Seq should return different Hash"
    ]

let tests_ResizeArray = 
    testList "ResizeArray" [
        testCase "Shuffled Int" <| fun _ ->               
            Expect.equal (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray1) "Same ResizeArray should return consistent Hash"
            Expect.equal (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray1') "Strucutally equal ResizeArray should return consistent Hash"
            Expect.notEqual (HashUtils.deepHash resizeArray1) (HashUtils.deepHash resizeArray2) "Different ResizeArray should return different Hash"
    ]


let tests_Mixed = 
    testList "Mixed" [
        testCase "List vs Dict" <| fun _ ->               
            Expect.notEqual (HashUtils.deepHash intList1) (HashUtils.deepHash intDict1) "List vs Dict with same values should return different Hash"
    ]




let main = testList "DeepHash" [
    tests_Dictionary
    tests_Lists
    tests_Array
    tests_Seq
    tests_ResizeArray
    tests_Mixed
]