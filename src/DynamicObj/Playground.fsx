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

let r1 = ResizeArray([1; 2])
let r2 = ResizeArray([1; 2])
let r3 = r1

printfn "%A" (LanguagePrimitives.PhysicalEquality r1 r2)
printfn "%A" (LanguagePrimitives.PhysicalEquality r2 r2)
printfn "%A" (LanguagePrimitives.PhysicalEquality r3 r1)