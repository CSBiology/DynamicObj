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

type T(dyn:string, stat:string) as this=
    inherit DynamicObj()

    do 
        this.SetProperty("Dyn", dyn)

    member this.Stat = stat

let first = T("dyn1", "stat1")
let second = T("dyn2", "stat2")

let _ = second.ShallowCopyDynamicPropertiesTo(first)

first |> DynObj.print
second |> DynObj.print