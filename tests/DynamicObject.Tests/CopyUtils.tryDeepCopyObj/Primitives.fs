module DeepCopyPrimitives

open System
open Fable.Pyxpecto
open DynamicObj
open Fable.Core
open TestUtils

let tests_DeepCopyPrimitives = testList "" [
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj true
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1uy
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1y
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1s
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1us
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1u
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1L
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1uL
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (System.IntPtr(1))
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj (System.UIntPtr(1u))
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1.0
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 1.0f
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj 'A'
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj "Hi"
        Expect.equal copy original "Expected values of copy and original to be equal"
    testCase "" <| fun _ -> 
        let original, copy = constructDeepCopiedObj ()
        Expect.equal copy original "Expected values of copy and original to be equal"
]