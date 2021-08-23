#load "ReflectionUtils.fs"
#load "DynamicObj.fs"
#load "DynObj.fs"

open DynamicObj

let foo = DynamicObj()
foo?bar <- [1;2;3;4]

(DynObj.print foo)
