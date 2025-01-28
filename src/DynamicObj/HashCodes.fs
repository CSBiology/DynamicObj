module DynamicObj.HashCodes

// Taken from
//https://softwareengineering.stackexchange.com/a/402543
// Which points to a no-longer existing source in FSharp Core Compiler
// But can be found in 
// https://github.com/dotnet/fsharp/blob/2edab1216843f20a00a7d8f171aca52cbc35d7fd/src/Compiler/Checking/AugmentWithHashCompare.fs#L171
// Or Fables mirror
// https://github.com/fable-compiler/Fable/blob/b0e640763fd90bd084f72531cb119d49a91ec077/src/fcs-fable/src/Compiler/Checking/AugmentWithHashCompare.fs#L171
let mergeHashes (hash1 : int) (hash2 : int) : int =
    0x9e3779b9 + hash2 + (hash1 <<< 6) + (hash1 >>> 2)

let hashDateTime (dt : System.DateTime) : int =
    let mutable acc = 0
    acc <- mergeHashes acc dt.Year
    acc <- mergeHashes acc dt.Month
    acc <- mergeHashes acc dt.Day
    acc <- mergeHashes acc dt.Hour
    acc <- mergeHashes acc dt.Minute
    acc <- mergeHashes acc dt.Second
    acc
    

let hash obj =
    if obj = null then 
        0
    else
        obj.GetHashCode()

let boxHashOption (a: 'a option) : obj =
    if a.IsSome then a.Value.GetHashCode() else (0).GetHashCode()
    |> box

let boxHashArray (a: 'a []) : obj =
    a 
    // from https://stackoverflow.com/a/53507559
    |> Array.fold (fun acc o -> 
        hash o
        |> mergeHashes acc) 0
    |> box

let boxHashSeq (a: seq<'a>) : obj =
    a 
    // from https://stackoverflow.com/a/53507559
    |> Seq.fold (fun acc o -> 
        hash o
        |> mergeHashes acc) 0
    |> box

let boxHashKeyValSeq (a: seq<System.Collections.Generic.KeyValuePair<'a,'b>>) : obj =
    a 
    // from https://stackoverflow.com/a/53507559
    |> Seq.fold (fun acc o -> 
        mergeHashes (hash o.Key) (hash o.Value)
        |> mergeHashes acc) 0
    |> box

let boxHashKeyValSeqBy (f : 'b -> int) (a: seq<System.Collections.Generic.KeyValuePair<'a,'b>>) : obj =
    a 
    // from https://stackoverflow.com/a/53507559
    |> Seq.fold (fun acc o -> 
        mergeHashes (hash o.Key) (f o.Value)
        |> mergeHashes acc) 0
    |> box