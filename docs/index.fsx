(*** condition: prepare ***)
#r "../bin/DynamicObj/netstandard2.0/DynamicObj.dll"

(*** condition: ipynb ***)
#if IPYNB
#r "nuget: DynamicObj, {{fsdocs-package-version}}"
#endif // IPYNB


(**
[![Binder]({{root}}img/badge-binder.svg)](https://mybinder.org/v2/gh/CSBiology/DynamicObj/gh-pages?filepath={{fsdocs-source-basename}}.ipynb)&emsp;
[![Script]({{root}}img/badge-script.svg)]({{root}}{{fsdocs-source-basename}}.fsx)&emsp;
[![Notebook]({{root}}img/badge-notebook.svg)]({{root}}{{fsdocs-source-basename}}.ipynb)

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
    |> JsonConvert.SerializeObject

(*** condition: ipynb ***)
#if IPYNB
aSerialized
#endif // IPYNB

(***include-value:aSerialized***)

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
    |> JsonConvert.SerializeObject

(*** condition: ipynb ***)
#if IPYNB
complexSerialized
#endif // IPYNB

(***include-value:complexSerialized***)