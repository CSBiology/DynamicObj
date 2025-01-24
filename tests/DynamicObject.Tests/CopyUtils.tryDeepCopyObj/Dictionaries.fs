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
    testList "sbyte keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<sbyte, bool>()
            d.Add(1y, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- false
            Expect.sequenceEqual original (dict [1y, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<sbyte, byte>()
            d.Add(1y, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2uy
            Expect.sequenceEqual original (dict [1y, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<sbyte, sbyte>()
            d.Add(1y, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2y
            Expect.sequenceEqual original (dict [1y, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<sbyte, int16>()
            d.Add(1y, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2s
            Expect.sequenceEqual original (dict [1y, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<sbyte, uint16>()
            d.Add(1y, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2us
            Expect.sequenceEqual original (dict [1y, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<sbyte, int>()
            d.Add(1y, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2
            Expect.sequenceEqual original (dict [1y, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<sbyte, uint>()
            d.Add(1y, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2u
            Expect.sequenceEqual original (dict [1y, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<sbyte, int64>()
            d.Add(1y, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2L
            Expect.sequenceEqual original (dict [1y, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<sbyte, uint64>()
            d.Add(1y, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2uL
            Expect.sequenceEqual original (dict [1y, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<sbyte, float>()
            d.Add(1y, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2.0
            Expect.sequenceEqual original (dict [1y, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<sbyte, float32>()
            d.Add(1y, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2.0f
            Expect.sequenceEqual original (dict [1y, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<sbyte, char>()
            d.Add(1y, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 'B'
            Expect.sequenceEqual original (dict [1y, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<sbyte, string>()
            d.Add(1y, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- "k2"
            Expect.sequenceEqual original (dict [1y, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<sbyte, unit>()
            d.Add(1y, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2y,())
            Expect.sequenceEqual original (dict [1y, (); 2y, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<sbyte, decimal>()
            d.Add(1y, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- 2.0M
            Expect.sequenceEqual original (dict [1y, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<sbyte, nativeint>()
            d.Add(1y, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1y, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<sbyte, unativeint>()
            d.Add(1y, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1y] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1y, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1y, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "int16 keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<int16, bool>()
            d.Add(1s, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- false
            Expect.sequenceEqual original (dict [1s, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<int16, byte>()
            d.Add(1s, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2uy
            Expect.sequenceEqual original (dict [1s, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<int16, sbyte>()
            d.Add(1s, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2y
            Expect.sequenceEqual original (dict [1s, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<int16, int16>()
            d.Add(1s, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2s
            Expect.sequenceEqual original (dict [1s, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<int16, uint16>()
            d.Add(1s, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2us
            Expect.sequenceEqual original (dict [1s, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<int16, int>()
            d.Add(1s, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2
            Expect.sequenceEqual original (dict [1s, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<int16, uint>()
            d.Add(1s, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2u
            Expect.sequenceEqual original (dict [1s, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<int16, int64>()
            d.Add(1s, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2L
            Expect.sequenceEqual original (dict [1s, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<int16, uint64>()
            d.Add(1s, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2uL
            Expect.sequenceEqual original (dict [1s, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<int16, float>()
            d.Add(1s, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2.0
            Expect.sequenceEqual original (dict [1s, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<int16, float32>()
            d.Add(1s, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2.0f
            Expect.sequenceEqual original (dict [1s, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<int16, char>()
            d.Add(1s, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 'B'
            Expect.sequenceEqual original (dict [1s, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<int16, string>()
            d.Add(1s, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- "k2"
            Expect.sequenceEqual original (dict [1s, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<int16, unit>()
            d.Add(1s, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2s,())
            Expect.sequenceEqual original (dict [1s, (); 2s, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<int16, decimal>()
            d.Add(1s, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- 2.0M
            Expect.sequenceEqual original (dict [1s, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<int16, nativeint>()
            d.Add(1s, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1s, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<int16, unativeint>()
            d.Add(1s, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1s] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1s, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1s, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "uint16 keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<uint16, bool>()
            d.Add(1us, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- false
            Expect.sequenceEqual original (dict [1us, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<uint16, byte>()
            d.Add(1us, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2uy
            Expect.sequenceEqual original (dict [1us, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<uint16, sbyte>()
            d.Add(1us, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2y
            Expect.sequenceEqual original (dict [1us, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<uint16, int16>()
            d.Add(1us, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2s
            Expect.sequenceEqual original (dict [1us, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<uint16, uint16>()
            d.Add(1us, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2us
            Expect.sequenceEqual original (dict [1us, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<uint16, int>()
            d.Add(1us, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2
            Expect.sequenceEqual original (dict [1us, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<uint16, uint>()
            d.Add(1us, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2u
            Expect.sequenceEqual original (dict [1us, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<uint16, int64>()
            d.Add(1us, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2L
            Expect.sequenceEqual original (dict [1us, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<uint16, uint64>()
            d.Add(1us, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2uL
            Expect.sequenceEqual original (dict [1us, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<uint16, float>()
            d.Add(1us, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2.0
            Expect.sequenceEqual original (dict [1us, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<uint16, float32>()
            d.Add(1us, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2.0f
            Expect.sequenceEqual original (dict [1us, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<uint16, char>()
            d.Add(1us, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 'B'
            Expect.sequenceEqual original (dict [1us, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<uint16, string>()
            d.Add(1us, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- "k2"
            Expect.sequenceEqual original (dict [1us, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<uint16, unit>()
            d.Add(1us, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2us,())
            Expect.sequenceEqual original (dict [1us, (); 2us, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<uint16, decimal>()
            d.Add(1us, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- 2.0M
            Expect.sequenceEqual original (dict [1us, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<uint16, nativeint>()
            d.Add(1us, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1us, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<uint16, unativeint>()
            d.Add(1us, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1us] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1us, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1us, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "int keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<int, bool>()
            d.Add(1, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- false
            Expect.sequenceEqual original (dict [1, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<int, byte>()
            d.Add(1, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2uy
            Expect.sequenceEqual original (dict [1, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<int, sbyte>()
            d.Add(1, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2y
            Expect.sequenceEqual original (dict [1, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<int, int16>()
            d.Add(1, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2s
            Expect.sequenceEqual original (dict [1, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<int, uint16>()
            d.Add(1, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2us
            Expect.sequenceEqual original (dict [1, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<int, int>()
            d.Add(1, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2
            Expect.sequenceEqual original (dict [1, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<int, uint>()
            d.Add(1, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2u
            Expect.sequenceEqual original (dict [1, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<int, int64>()
            d.Add(1, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2L
            Expect.sequenceEqual original (dict [1, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<int, uint64>()
            d.Add(1, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2uL
            Expect.sequenceEqual original (dict [1, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<int, float>()
            d.Add(1, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2.0
            Expect.sequenceEqual original (dict [1, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<int, float32>()
            d.Add(1, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2.0f
            Expect.sequenceEqual original (dict [1, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<int, char>()
            d.Add(1, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 'B'
            Expect.sequenceEqual original (dict [1, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<int, string>()
            d.Add(1, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- "k2"
            Expect.sequenceEqual original (dict [1, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<int, unit>()
            d.Add(1, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2,())
            Expect.sequenceEqual original (dict [1, (); 2, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<int, decimal>()
            d.Add(1, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- 2.0M
            Expect.sequenceEqual original (dict [1, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<int, nativeint>()
            d.Add(1, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<int, unativeint>()
            d.Add(1, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "uint keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<uint, bool>()
            d.Add(1u, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- false
            Expect.sequenceEqual original (dict [1u, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<uint, byte>()
            d.Add(1u, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2uy
            Expect.sequenceEqual original (dict [1u, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<uint, sbyte>()
            d.Add(1u, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2y
            Expect.sequenceEqual original (dict [1u, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<uint, int16>()
            d.Add(1u, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2s
            Expect.sequenceEqual original (dict [1u, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<uint, uint16>()
            d.Add(1u, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2us
            Expect.sequenceEqual original (dict [1u, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<uint, int>()
            d.Add(1u, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2
            Expect.sequenceEqual original (dict [1u, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<uint, uint>()
            d.Add(1u, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2u
            Expect.sequenceEqual original (dict [1u, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<uint, int64>()
            d.Add(1u, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2L
            Expect.sequenceEqual original (dict [1u, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<uint, uint64>()
            d.Add(1u, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2uL
            Expect.sequenceEqual original (dict [1u, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<uint, float>()
            d.Add(1u, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2.0
            Expect.sequenceEqual original (dict [1u, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<uint, float32>()
            d.Add(1u, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2.0f
            Expect.sequenceEqual original (dict [1u, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<uint, char>()
            d.Add(1u, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 'B'
            Expect.sequenceEqual original (dict [1u, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<uint, string>()
            d.Add(1u, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- "k2"
            Expect.sequenceEqual original (dict [1u, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<uint, unit>()
            d.Add(1u, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2u,())
            Expect.sequenceEqual original (dict [1u, (); 2u, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<uint, decimal>()
            d.Add(1u, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- 2.0M
            Expect.sequenceEqual original (dict [1u, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<uint, nativeint>()
            d.Add(1u, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1u, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<uint, unativeint>()
            d.Add(1u, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1u] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1u, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1u, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "int64 keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<int64, bool>()
            d.Add(1L, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- false
            Expect.sequenceEqual original (dict [1L, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<int64, byte>()
            d.Add(1L, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2uy
            Expect.sequenceEqual original (dict [1L, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<int64, sbyte>()
            d.Add(1L, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2y
            Expect.sequenceEqual original (dict [1L, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<int64, int16>()
            d.Add(1L, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2s
            Expect.sequenceEqual original (dict [1L, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<int64, uint16>()
            d.Add(1L, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2us
            Expect.sequenceEqual original (dict [1L, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<int64, int>()
            d.Add(1L, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2
            Expect.sequenceEqual original (dict [1L, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<int64, uint>()
            d.Add(1L, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2u
            Expect.sequenceEqual original (dict [1L, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<int64, int64>()
            d.Add(1L, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2L
            Expect.sequenceEqual original (dict [1L, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<int64, uint64>()
            d.Add(1L, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2uL
            Expect.sequenceEqual original (dict [1L, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<int64, float>()
            d.Add(1L, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2.0
            Expect.sequenceEqual original (dict [1L, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<int64, float32>()
            d.Add(1L, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2.0f
            Expect.sequenceEqual original (dict [1L, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<int64, char>()
            d.Add(1L, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 'B'
            Expect.sequenceEqual original (dict [1L, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<int64, string>()
            d.Add(1L, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- "k2"
            Expect.sequenceEqual original (dict [1L, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<int64, unit>()
            d.Add(1L, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2L,())
            Expect.sequenceEqual original (dict [1L, (); 2L, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<int64, decimal>()
            d.Add(1L, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- 2.0M
            Expect.sequenceEqual original (dict [1L, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<int64, nativeint>()
            d.Add(1L, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1L, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<int64, unativeint>()
            d.Add(1L, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1L] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1L, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1L, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "uint64 keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<uint64, bool>()
            d.Add(1uL, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- false
            Expect.sequenceEqual original (dict [1uL, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<uint64, byte>()
            d.Add(1uL, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2uy
            Expect.sequenceEqual original (dict [1uL, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<uint64, sbyte>()
            d.Add(1uL, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2y
            Expect.sequenceEqual original (dict [1uL, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<uint64, int16>()
            d.Add(1uL, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2s
            Expect.sequenceEqual original (dict [1uL, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<uint64, uint16>()
            d.Add(1uL, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2us
            Expect.sequenceEqual original (dict [1uL, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<uint64, int>()
            d.Add(1uL, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2
            Expect.sequenceEqual original (dict [1uL, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<uint64, uint>()
            d.Add(1uL, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2u
            Expect.sequenceEqual original (dict [1uL, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<uint64, int64>()
            d.Add(1uL, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2L
            Expect.sequenceEqual original (dict [1uL, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<uint64, uint64>()
            d.Add(1uL, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2uL
            Expect.sequenceEqual original (dict [1uL, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<uint64, float>()
            d.Add(1uL, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2.0
            Expect.sequenceEqual original (dict [1uL, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<uint64, float32>()
            d.Add(1uL, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2.0f
            Expect.sequenceEqual original (dict [1uL, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<uint64, char>()
            d.Add(1uL, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 'B'
            Expect.sequenceEqual original (dict [1uL, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<uint64, string>()
            d.Add(1uL, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- "k2"
            Expect.sequenceEqual original (dict [1uL, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<uint64, unit>()
            d.Add(1uL, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2uL,())
            Expect.sequenceEqual original (dict [1uL, (); 2uL, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<uint64, decimal>()
            d.Add(1uL, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- 2.0M
            Expect.sequenceEqual original (dict [1uL, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<uint64, nativeint>()
            d.Add(1uL, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1uL, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<uint64, unativeint>()
            d.Add(1uL, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1uL] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1uL, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1uL, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "float keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<float, bool>()
            d.Add(1.0, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- false
            Expect.sequenceEqual original (dict [1.0, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<float, byte>()
            d.Add(1.0, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2uy
            Expect.sequenceEqual original (dict [1.0, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<float, sbyte>()
            d.Add(1.0, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2y
            Expect.sequenceEqual original (dict [1.0, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<float, int16>()
            d.Add(1.0, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2s
            Expect.sequenceEqual original (dict [1.0, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<float, uint16>()
            d.Add(1.0, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2us
            Expect.sequenceEqual original (dict [1.0, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<float, int>()
            d.Add(1.0, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2
            Expect.sequenceEqual original (dict [1.0, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<float, uint>()
            d.Add(1.0, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2u
            Expect.sequenceEqual original (dict [1.0, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<float, int64>()
            d.Add(1.0, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2L
            Expect.sequenceEqual original (dict [1.0, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<float, uint64>()
            d.Add(1.0, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2uL
            Expect.sequenceEqual original (dict [1.0, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<float, float>()
            d.Add(1.0, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2.0
            Expect.sequenceEqual original (dict [1.0, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<float, float32>()
            d.Add(1.0, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2.0f
            Expect.sequenceEqual original (dict [1.0, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<float, char>()
            d.Add(1.0, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 'B'
            Expect.sequenceEqual original (dict [1.0, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<float, string>()
            d.Add(1.0, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- "k2"
            Expect.sequenceEqual original (dict [1.0, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<float, unit>()
            d.Add(1.0, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2.0,())
            Expect.sequenceEqual original (dict [1.0, (); 2.0, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<float, decimal>()
            d.Add(1.0, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- 2.0M
            Expect.sequenceEqual original (dict [1.0, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<float, nativeint>()
            d.Add(1.0, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1.0, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<float, unativeint>()
            d.Add(1.0, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1.0, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "float32 keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<float32, bool>()
            d.Add(1.0f, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- false
            Expect.sequenceEqual original (dict [1.0f, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<float32, byte>()
            d.Add(1.0f, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2uy
            Expect.sequenceEqual original (dict [1.0f, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<float32, sbyte>()
            d.Add(1.0f, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2y
            Expect.sequenceEqual original (dict [1.0f, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<float32, int16>()
            d.Add(1.0f, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2s
            Expect.sequenceEqual original (dict [1.0f, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<float32, uint16>()
            d.Add(1.0f, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2us
            Expect.sequenceEqual original (dict [1.0f, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<float32, int>()
            d.Add(1.0f, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2
            Expect.sequenceEqual original (dict [1.0f, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<float32, uint>()
            d.Add(1.0f, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2u
            Expect.sequenceEqual original (dict [1.0f, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<float32, int64>()
            d.Add(1.0f, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2L
            Expect.sequenceEqual original (dict [1.0f, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<float32, uint64>()
            d.Add(1.0f, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2uL
            Expect.sequenceEqual original (dict [1.0f, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<float32, float>()
            d.Add(1.0f, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2.0
            Expect.sequenceEqual original (dict [1.0f, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<float32, float32>()
            d.Add(1.0f, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2.0f
            Expect.sequenceEqual original (dict [1.0f, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<float32, char>()
            d.Add(1.0f, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 'B'
            Expect.sequenceEqual original (dict [1.0f, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<float32, string>()
            d.Add(1.0f, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- "k2"
            Expect.sequenceEqual original (dict [1.0f, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<float32, unit>()
            d.Add(1.0f, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2.0f,())
            Expect.sequenceEqual original (dict [1.0f, (); 2.0f, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<float32, decimal>()
            d.Add(1.0f, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- 2.0M
            Expect.sequenceEqual original (dict [1.0f, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<float32, nativeint>()
            d.Add(1.0f, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1.0f, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<float32, unativeint>()
            d.Add(1.0f, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0f] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1.0f, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0f, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "char keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<char, bool>()
            d.Add('A', true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- false
            Expect.sequenceEqual original (dict ['A', false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<char, byte>()
            d.Add('A', 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2uy
            Expect.sequenceEqual original (dict ['A', 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<char, sbyte>()
            d.Add('A', 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2y
            Expect.sequenceEqual original (dict ['A', 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<char, int16>()
            d.Add('A', 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2s
            Expect.sequenceEqual original (dict ['A', 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<char, uint16>()
            d.Add('A', 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2us
            Expect.sequenceEqual original (dict ['A', 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<char, int>()
            d.Add('A', 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2
            Expect.sequenceEqual original (dict ['A', 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<char, uint>()
            d.Add('A', 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2u
            Expect.sequenceEqual original (dict ['A', 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<char, int64>()
            d.Add('A', 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2L
            Expect.sequenceEqual original (dict ['A', 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<char, uint64>()
            d.Add('A', 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2uL
            Expect.sequenceEqual original (dict ['A', 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<char, float>()
            d.Add('A', 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2.0
            Expect.sequenceEqual original (dict ['A', 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<char, float32>()
            d.Add('A', 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2.0f
            Expect.sequenceEqual original (dict ['A', 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<char, char>()
            d.Add('A', 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 'B'
            Expect.sequenceEqual original (dict ['A', 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<char, string>()
            d.Add('A', "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- "k2"
            Expect.sequenceEqual original (dict ['A', "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<char, unit>()
            d.Add('A', ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add('B',())
            Expect.sequenceEqual original (dict ['A', (); 'B', ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<char, decimal>()
            d.Add('A', 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- 2.0M
            Expect.sequenceEqual original (dict ['A', 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<char, nativeint>()
            d.Add('A', System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict ['A', System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<char, unativeint>()
            d.Add('A', System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.['A'] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict ['A', System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ['A', System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "string keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<string, bool>()
            d.Add("Hi", true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- false
            Expect.sequenceEqual original (dict ["Hi", false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<string, byte>()
            d.Add("Hi", 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2uy
            Expect.sequenceEqual original (dict ["Hi", 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<string, sbyte>()
            d.Add("Hi", 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2y
            Expect.sequenceEqual original (dict ["Hi", 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<string, int16>()
            d.Add("Hi", 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2s
            Expect.sequenceEqual original (dict ["Hi", 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<string, uint16>()
            d.Add("Hi", 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2us
            Expect.sequenceEqual original (dict ["Hi", 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<string, int>()
            d.Add("Hi", 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2
            Expect.sequenceEqual original (dict ["Hi", 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<string, uint>()
            d.Add("Hi", 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2u
            Expect.sequenceEqual original (dict ["Hi", 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<string, int64>()
            d.Add("Hi", 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2L
            Expect.sequenceEqual original (dict ["Hi", 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<string, uint64>()
            d.Add("Hi", 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2uL
            Expect.sequenceEqual original (dict ["Hi", 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<string, float>()
            d.Add("Hi", 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2.0
            Expect.sequenceEqual original (dict ["Hi", 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<string, float32>()
            d.Add("Hi", 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2.0f
            Expect.sequenceEqual original (dict ["Hi", 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<string, char>()
            d.Add("Hi", 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 'B'
            Expect.sequenceEqual original (dict ["Hi", 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<string, string>()
            d.Add("Hi", "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- "k2"
            Expect.sequenceEqual original (dict ["Hi", "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<string, unit>()
            d.Add("Hi", ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add("Bye",())
            Expect.sequenceEqual original (dict ["Hi", (); "Bye", ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<string, decimal>()
            d.Add("Hi", 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- 2.0M
            Expect.sequenceEqual original (dict ["Hi", 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<string, nativeint>()
            d.Add("Hi", System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict ["Hi", System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<string, unativeint>()
            d.Add("Hi", System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.["Hi"] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict ["Hi", System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict ["Hi", System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    //testList "unit keys" [
    //not testing this, the concept is ridiculous
    //]
    #if !FABLE_COMPILER_PYTHON
    testList "decimal keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<decimal, bool>()
            d.Add(1.0M, true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- false
            Expect.sequenceEqual original (dict [1.0M, false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<decimal, byte>()
            d.Add(1.0M, 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2uy
            Expect.sequenceEqual original (dict [1.0M, 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<decimal, sbyte>()
            d.Add(1.0M, 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2y
            Expect.sequenceEqual original (dict [1.0M, 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<decimal, int16>()
            d.Add(1.0M, 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2s
            Expect.sequenceEqual original (dict [1.0M, 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<decimal, uint16>()
            d.Add(1.0M, 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2us
            Expect.sequenceEqual original (dict [1.0M, 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<decimal, int>()
            d.Add(1.0M, 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2
            Expect.sequenceEqual original (dict [1.0M, 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<decimal, uint>()
            d.Add(1.0M, 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2u
            Expect.sequenceEqual original (dict [1.0M, 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<decimal, int64>()
            d.Add(1.0M, 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2L
            Expect.sequenceEqual original (dict [1.0M, 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<decimal, uint64>()
            d.Add(1.0M, 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2uL
            Expect.sequenceEqual original (dict [1.0M, 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<decimal, float>()
            d.Add(1.0M, 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2.0
            Expect.sequenceEqual original (dict [1.0M, 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<decimal, float32>()
            d.Add(1.0M, 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2.0f
            Expect.sequenceEqual original (dict [1.0M, 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<decimal, char>()
            d.Add(1.0M, 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 'B'
            Expect.sequenceEqual original (dict [1.0M, 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<decimal, string>()
            d.Add(1.0M, "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- "k2"
            Expect.sequenceEqual original (dict [1.0M, "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<decimal, unit>()
            d.Add(1.0M, ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(2.0M,())
            Expect.sequenceEqual original (dict [1.0M, (); 2.0M, ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<decimal, decimal>()
            d.Add(1.0M, 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- 2.0M
            Expect.sequenceEqual original (dict [1.0M, 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<decimal, nativeint>()
            d.Add(1.0M, System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [1.0M, System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<decimal, unativeint>()
            d.Add(1.0M, System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[1.0M] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [1.0M, System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [1.0M, System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    #endif
    #if !FABLE_COMPILER
    testList "nativeint keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<nativeint, bool>()
            d.Add(System.IntPtr(1), true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- false
            Expect.sequenceEqual original (dict [System.IntPtr(1), false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<nativeint, byte>()
            d.Add(System.IntPtr(1), 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2uy
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<nativeint, sbyte>()
            d.Add(System.IntPtr(1), 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2y
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<nativeint, int16>()
            d.Add(System.IntPtr(1), 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2s
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<nativeint, uint16>()
            d.Add(System.IntPtr(1), 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2us
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<nativeint, int>()
            d.Add(System.IntPtr(1), 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<nativeint, uint>()
            d.Add(System.IntPtr(1), 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2u
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<nativeint, int64>()
            d.Add(System.IntPtr(1), 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2L
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<nativeint, uint64>()
            d.Add(System.IntPtr(1), 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2uL
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<nativeint, float>()
            d.Add(System.IntPtr(1), 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2.0
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<nativeint, float32>()
            d.Add(System.IntPtr(1), 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2.0f
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<nativeint, char>()
            d.Add(System.IntPtr(1), 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 'B'
            Expect.sequenceEqual original (dict [System.IntPtr(1), 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<nativeint, string>()
            d.Add(System.IntPtr(1), "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- "k2"
            Expect.sequenceEqual original (dict [System.IntPtr(1), "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<nativeint, unit>()
            d.Add(System.IntPtr(1), ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(System.IntPtr(2),())
            Expect.sequenceEqual original (dict [System.IntPtr(1), (); System.IntPtr(2), ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<nativeint, decimal>()
            d.Add(System.IntPtr(1), 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- 2.0M
            Expect.sequenceEqual original (dict [System.IntPtr(1), 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<nativeint, nativeint>()
            d.Add(System.IntPtr(1), System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [System.IntPtr(1), System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<nativeint, unativeint>()
            d.Add(System.IntPtr(1), System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.IntPtr(1)] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [System.IntPtr(1), System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.IntPtr(1), System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    testList "unativeint keys" [
        testCase "bool values" <| fun _ ->
            let d = new Dictionary<unativeint, bool>()
            d.Add(System.UIntPtr(1u), true)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- false
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), false]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), true]) "Clone should not be affected by original mutation"
        testCase "byte values" <| fun _ ->       
            let d = new Dictionary<unativeint, byte>()
            d.Add(System.UIntPtr(1u), 1uy)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2uy
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2uy]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1uy]) "Clone should not be affected by original mutation"
        testCase "sbyte values" <| fun _ ->      
            let d = new Dictionary<unativeint, sbyte>()
            d.Add(System.UIntPtr(1u), 1y)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2y
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2y]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1y]) "Clone should not be affected by original mutation"
        testCase "int16 values" <| fun _ ->      
            let d = new Dictionary<unativeint, int16>()
            d.Add(System.UIntPtr(1u), 1s)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2s
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2s]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1s]) "Clone should not be affected by original mutation"
        testCase "uint16 values" <| fun _ ->     
            let d = new Dictionary<unativeint, uint16>()
            d.Add(System.UIntPtr(1u), 1us)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2us
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2us]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1us]) "Clone should not be affected by original mutation"
        testCase "int values" <| fun _ ->        
            let d = new Dictionary<unativeint, int>()
            d.Add(System.UIntPtr(1u), 1)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1]) "Clone should not be affected by original mutation"
        testCase "uint values" <| fun _ ->       
            let d = new Dictionary<unativeint, uint>()
            d.Add(System.UIntPtr(1u), 1u)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2u
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2u]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1u]) "Clone should not be affected by original mutation"
        testCase "int64 values" <| fun _ ->      
            let d = new Dictionary<unativeint, int64>()
            d.Add(System.UIntPtr(1u), 1L)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2L
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2L]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1L]) "Clone should not be affected by original mutation"
        testCase "uint64 values" <| fun _ ->     
            let d = new Dictionary<unativeint, uint64>()
            d.Add(System.UIntPtr(1u), 1uL)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2uL
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2uL]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1uL]) "Clone should not be affected by original mutation"
        testCase "float values" <| fun _ ->      
            let d = new Dictionary<unativeint, float>()
            d.Add(System.UIntPtr(1u), 1.0)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2.0
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2.0]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1.0]) "Clone should not be affected by original mutation"
        testCase "float32 values" <| fun _ ->    
            let d = new Dictionary<unativeint, float32>()
            d.Add(System.UIntPtr(1u), 1.0f)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2.0f
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2.0f]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1.0f]) "Clone should not be affected by original mutation"
        testCase "char values" <| fun _ ->       
            let d = new Dictionary<unativeint, char>()
            d.Add(System.UIntPtr(1u), 'A')
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 'B'
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 'B']) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 'A']) "Clone should not be affected by original mutation"
        testCase "string values" <| fun _ ->     
            let d = new Dictionary<unativeint, string>()
            d.Add(System.UIntPtr(1u), "k1")
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- "k2"
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), "k2"]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), "k1"]) "Clone should not be affected by original mutation"
        testCase "unit values" <| fun _ ->       
            let d = new Dictionary<unativeint, unit>()
            d.Add(System.UIntPtr(1u), ())
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.Add(System.UIntPtr(2u),())
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), (); System.UIntPtr(2u), ()]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), ()]) "Clone should not be affected by original mutation"
        #if !FABLE_COMPILER_PYTHON
        testCase "decimal values" <| fun _ ->    
            let d = new Dictionary<unativeint, decimal>()
            d.Add(System.UIntPtr(1u), 1.0M)
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- 2.0M
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), 2.0M]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), 1.0M]) "Clone should not be affected by original mutation"
        #endif
        #if !FABLE_COMPILER
        testCase "nativeint values" <| fun _ ->  
            let d = new Dictionary<unativeint, nativeint>()
            d.Add(System.UIntPtr(1u), System.IntPtr(1))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- System.IntPtr(2)
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), System.IntPtr(2)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), System.IntPtr(1)]) "Clone should not be affected by original mutation"
        testCase "unativeint values" <| fun _ -> 
            let d = new Dictionary<unativeint, unativeint>()
            d.Add(System.UIntPtr(1u), System.UIntPtr(1u))
            let original, copy = constructDeepCopiedObj d
            Expect.sequenceEqual copy original "Expected values of copy and original to be equal"
            Expect.notReferenceEqual copy original "Expected values of copy and original to be not reference equal"
            d.[System.UIntPtr(1u)] <- System.UIntPtr(2u)
            Expect.sequenceEqual original (dict [System.UIntPtr(1u), System.UIntPtr(2u)]) "Original schould have been mutated"
            Expect.sequenceEqual copy (dict [System.UIntPtr(1u), System.UIntPtr(1u)]) "Clone should not be affected by original mutation"
        #endif
    ]
    #endif
]