#r "nuget: Newtonsoft.Json"
#r "nuget: Fable.Core"
#r "nuget: Fable.Pyxpecto"

#load "./HashCodes.fs"
#load "./PropertyHelper.fs"
#load "./FablePy.fs"
#load "./FableJS.fs"
#load "./ReflectionUtils.fs"
#load "./DynamicObj.fs"
#load "./DynObj.fs"

open Fable.Pyxpecto
open DynamicObj

let constructDeepCopiedClone<'T> (props: seq<string*obj>) =
    let original = DynamicObj()
    props
    |> Seq.iter (fun (propertyName, propertyValue) -> original.SetProperty(propertyName, propertyValue))
    let clone : 'T = original.DeepCopyProperties() |> unbox<'T>
    original, clone 

let originalProps = [ 
    "int", box 1
    "float", box 1.0
    "bool", box true
    "string", box "hello"
    "char", box 'a'
    "byte", box (byte 1)
    "sbyte", box (sbyte -1)
    "int16", box (int16 -1)
    "uint16", box (uint16 1)
    "int32", box (int32 -1)
    "uint32", box (uint32 1u)
    "int64", box (int64 -1L)
    "uint64", box (uint64 1UL)
    "single", box (single 1.0f)
    "decimal", box (decimal 1M) 
]
let original = DynamicObj()

originalProps
|> Seq.iter (fun (propertyName, propertyValue) -> original.SetProperty(propertyName, propertyValue))

let clone = original.DeepCopyProperties()