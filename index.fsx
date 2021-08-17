(**
[![Binder](https://github.com/CSBiology/DynamicObj/img/badge-binder.svg)](https://mybinder.org/v2/gh/CSBiology/DynamicObj/gh-pages?filepath=index.ipynb)&emsp;
[![Script](https://github.com/CSBiology/DynamicObj/img/badge-script.svg)](https://github.com/CSBiology/DynamicObj/index.fsx)&emsp;
[![Notebook](https://github.com/CSBiology/DynamicObj/img/badge-notebook.svg)](https://github.com/CSBiology/DynamicObj/index.ipynb)

# DynamicObj

F# library supporting Dynamic Objects including inheritance in functional style. It builds on ´System.Dynamic´ but adds object inheritance.

One main use case of this library is the dynamic generation of JSON objects - especially nested objects with optional properties - aimed to be used from javascript wuith the init/style pattern:

Use it for your custom types via inheritance:


*)
#r "nuget: Newtonsoft.JSON, 12.0.3"
open Newtonsoft.Json
open DynamicObj

type A() = 
    inherit DynamicObj()

    static member init
        (
            ?SomeProp: int
        ) =
            A()
            |> A.style
                (
                    ?SomeProp  = SomeProp
                )

    static member style
        (
            ?SomeProp: int
        ) =
            fun (a:A) ->

                SomeProp |> DynObj.setValueOpt a "some_prop"

                a


let aSerialized =
    A.init(42)
    |> JsonConvert.SerializeObject(* output: 
"{"some_prop":42}"*)
type MyComplexJSONType() =
    inherit DynamicObj()

    static member init
        (
            ?PropA: int [],
            ?PropB: A
        ) =
            MyComplexJSONType()
            |> MyComplexJSONType.style
                (
                    ?PropA  = PropA,
                    ?PropB  = PropB
                )

    static member style
        (
            ?PropA: int [],
            ?PropB: A
        ) =
            fun (t:MyComplexJSONType) ->

                PropA |> DynObj.setValueOpt t "prop_a"
                PropB |> DynObj.setValueOpt t "prop_b"

                t

let complexSerialized =
    MyComplexJSONType.init(
        PropA = [|42;1337|],
        PropB = A.init(68) // nested dynamic objects
    )
    |> JsonConvert.SerializeObject(* output: 
"{"prop_a":[42,1337],"prop_b":{"some_prop":68}}"*)

