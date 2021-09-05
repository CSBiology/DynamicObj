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

F# library supporting Dynamic Objects including inheritance in functional style.

#### Table of contents

- [DynamicObj (mutable)](#DynamicObj-mutable)
    - [Simple inheritance pattern for DynamicObj](#Simple-inheritance-pattern-for-DynamicObj)
    - [Nesting DynamicObjs](#Nesting-DynamicObjs)
- [ImmutableDynamicObj](#ImmutableDynamicObj)
    - [Simple inheritance pattern for ImmutableDynamicObj](#Simple-inheritance-pattern-for-ImmutableDynamicObj)
    - [Nesting ImmutableDynamicObjs](#Nesting-ImmutableDynamicObjs)

# DynamicObj (mutable)

`DynamicObj` builds on ´System.Dynamic´ but adds object inheritance.

One main use case of this library is the dynamic generation of JSON objects - especially nested objects with optional properties - aimed to be used from javascript wuith the init/style pattern:

Use it for your custom types via inheritance:

## Simple inheritance pattern for DynamicObj

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

(**
You can use the `DynObj.print` function to look at the dynamic members of the object:
*)

A.init(42) |> DynObj.print

(***include-output***)

(**
And this is how the serialized JSON looks like:
*)


let aSerialized =
    A.init(42)
    |> JsonConvert.SerializeObject

(*** condition: ipynb ***)
#if IPYNB
aSerialized
#endif // IPYNB

(***include-value:aSerialized***)

(**
## Nesting DynamicObjs
*)

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

let complex =
    MyComplexJSONType.init(
        PropA = [|42;1337|],
        PropB = A.init(68) // nested dynamic objects
    )

let complexSerialized = 
    complex
    |> JsonConvert.SerializeObject

(**
You can use the `DynObj.print` function to look at the dynamic members of the object:
*)

complex |> DynObj.print

(***include-output***)

(**
And this is how the serialized JSON looks like:
*)

(*** condition: ipynb ***)
#if IPYNB
complexSerialized
#endif // IPYNB

(***include-value:complexSerialized***)

(**

*)

(**
## Simple inheritance pattern for ImmutableDynamicObj

*)
#r "nuget: Newtonsoft.JSON, 12.0.3"
open Newtonsoft.Json
open DynamicObj

type ImmutableA() = 
    inherit ImmutableDynamicObj()

    static member init
        (
            ?SomeProp: int
        ) =
            ImmutableA()
            |> ImmutableA.style
                (
                    ?SomeProp  = SomeProp
                )

    static member style
        (
            ?SomeProp: int
        ) =
            fun (a:ImmutableA) ->
                a 
                |> ImmutableDynamicObj.addOpt "some_prop" SomeProp


(**
You can use the `ImmutableDynamicObj.print` function to look at the dynamic members of the object:
*)

ImmutableA.init(42) |> ImmutableDynamicObj.print

(***include-output***)

(**
And this is how the serialized JSON looks like:
*)


let immutableASerialized =
    ImmutableA.init(42)
    |> JsonConvert.SerializeObject

(*** condition: ipynb ***)
#if IPYNB
immutableASerialized
#endif // IPYNB

(***include-value:immutableASerialized***)

(**
## Nesting DynamicObjs

`DynamicObj.Operators` adds usefull operators for adding properties:
*)

open DynamicObj.Operators

type ImmutableMyComplexJSONType() =
    inherit ImmutableDynamicObj()

    static member init
        (
            ?PropA: int [],
            ?PropB: A
        ) =
            ImmutableMyComplexJSONType()
            |> ImmutableMyComplexJSONType.style
                (
                    ?PropA  = PropA,
                    ?PropB  = PropB
                )

    static member style
        (
            ?PropA: int [],
            ?PropB: A
        ) =
            fun (t:ImmutableMyComplexJSONType) ->
                t
                ++? ("prop_a", PropA)
                ++? ("prop_b", PropB)

let immutableComplex =
    ImmutableMyComplexJSONType.init(
        PropA = [|42;1337|],
        PropB = A.init(68) // nested dynamic objects
    )

let immutableComplexSerialized = 
    immutableComplex
    |> JsonConvert.SerializeObject

(**
You can use the `DynObj.print` function to look at the dynamic members of the object:
*)

complex |> DynObj.print

(***include-output***)

(**
And this is how the serialized JSON looks like:
*)

(*** condition: ipynb ***)
#if IPYNB
immutableComplexSerialized
#endif // IPYNB

(***include-value:immutableComplexSerialized***)

(**

*)