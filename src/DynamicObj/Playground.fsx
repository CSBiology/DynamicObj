#r "nuget: Newtonsoft.Json"
#r "nuget: Fable.Core"
#r "nuget: Fable.Pyxpecto"

#load "./ReflectionUtils.fs"
#load "./DynamicObj.fs"
#load "./DynObj.fs"

open Fable.Pyxpecto
open DynamicObj


let a = DynamicObj ()
a.SetValue("aaa", 5)
let b = DynamicObj ()
b.SetValue("aaa", 5)


a.GetProperties(true)