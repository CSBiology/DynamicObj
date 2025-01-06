module DeepCopyDictionaries

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core
open TestUtils
open System.Collections.Generic

let tests_DeepCopyDictionaries = testList "Dictionaries" [
// there are hundreds of potential test cases here for each type combination
    testList "bool keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<bool, bool>()
            d.Add(true, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- false
            Expect.sequenceEqual original (dict [true, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<bool, byte>()
            d.Add(true, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2uy
            Expect.sequenceEqual original (dict [true, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<bool, sbyte>()
            d.Add(true, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2y
            Expect.sequenceEqual original (dict [true, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<bool, int16>()
            d.Add(true, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2s
            Expect.sequenceEqual original (dict [true, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<bool, uint16>()
            d.Add(true, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2us
            Expect.sequenceEqual original (dict [true, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<bool, int>()
            d.Add(true, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2
            Expect.sequenceEqual original (dict [true, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<bool, uint>()
            d.Add(true, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2u
            Expect.sequenceEqual original (dict [true, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<bool, int64>()
            d.Add(true, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2L
            Expect.sequenceEqual original (dict [true, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<bool, uint64>()
            d.Add(true, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2uL
            Expect.sequenceEqual original (dict [true, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<bool, float>()
            d.Add(true, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2.0
            Expect.sequenceEqual original (dict [true, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<bool, float32>()
            d.Add(true, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2.0f
            Expect.sequenceEqual original (dict [true, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<bool, char>()
            d.Add(true, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 'B'
            Expect.sequenceEqual original (dict [true, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<bool, string>()
            d.Add(true, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- "k2"
            Expect.sequenceEqual original (dict [true, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<bool, unit>()
            d.Add(true, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(false,())
            Expect.sequenceEqual original (dict [true, (); false, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<bool, decimal>()
            d.Add(true, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- 2.0M
            Expect.sequenceEqual original (dict [true, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<bool, nativeint>()
            d.Add(true, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [true, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<bool, unativeint>()
            d.Add(true, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[true] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [true, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [true, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "byte keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<byte, bool>()
            d.Add(1uy, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- false
            Expect.sequenceEqual original (dict [1uy, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<byte, byte>()
            d.Add(1uy, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2uy
            Expect.sequenceEqual original (dict [1uy, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<byte, sbyte>()
            d.Add(1uy, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2y
            Expect.sequenceEqual original (dict [1uy, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<byte, int16>()
            d.Add(1uy, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2s
            Expect.sequenceEqual original (dict [1uy, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<byte, uint16>()
            d.Add(1uy, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2us
            Expect.sequenceEqual original (dict [1uy, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<byte, int>()
            d.Add(1uy, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2
            Expect.sequenceEqual original (dict [1uy, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<byte, uint>()
            d.Add(1uy, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2u
            Expect.sequenceEqual original (dict [1uy, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<byte, int64>()
            d.Add(1uy, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2L
            Expect.sequenceEqual original (dict [1uy, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<byte, uint64>()
            d.Add(1uy, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2uL
            Expect.sequenceEqual original (dict [1uy, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<byte, float>()
            d.Add(1uy, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2.0
            Expect.sequenceEqual original (dict [1uy, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<byte, float32>()
            d.Add(1uy, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2.0f
            Expect.sequenceEqual original (dict [1uy, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<byte, char>()
            d.Add(1uy, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 'B'
            Expect.sequenceEqual original (dict [1uy, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<byte, string>()
            d.Add(1uy, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- "k2"
            Expect.sequenceEqual original (dict [1uy, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<byte, unit>()
            d.Add(1uy, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2uy,())
            Expect.sequenceEqual original (dict [1uy, (); 2uy, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<byte, decimal>()
            d.Add(1uy, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- 2.0M
            Expect.sequenceEqual original (dict [1uy, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<byte, nativeint>()
            d.Add(1uy, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1uy, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<byte, unativeint>()
            d.Add(1uy, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uy] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1uy, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uy, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    //testList "byte keys" [

    //]
    //testList "sbyte keys" [

    //]
    //testList "int16 keys" [

    //]
    //testList "uint16 keys" [

    //]
    //testList "int keys" [

    //]
    //testList "uint keys" [

    //]
    //testList "int64 keys" [

    //]
    //testList "uint64 keys" [

    //]
    //testList "float keys" [

    //]
    //testList "float32 keys" [

    //]
    //testList "char keys" [

    //]
    //testList "string keys" [

    //]
    //testList "unit keys" [

    //]
    #if !FABLE_COMPILER_PYTHON
    //testList "decimal keys" [

    //]
    #endif
    #if !FABLE_COMPILER
    //testList "nativeint keys" [

    //]
    //testList "unativeint keys" [

    //]
    #endif
]