module TestUtils

open System
open DynamicObj
open Fable.Core

[<AttachMembers>]
type MutableClass(stat:string) =     
    let mutable s = stat
    member this.stat with get() = s and set v = s <- v

[<AttachMembers>]
type DerivedClass(stat: string, dyn: string) as this =
    inherit DynamicObj()
    do
        this.SetProperty("dyn", dyn)
    member this.stat = stat

[<AttachMembers>]
type DerivedClassCloneable(stat: string, dyn: string) as this =
    inherit DynamicObj()
    do
        this.SetProperty("dyn", dyn)
    member this.stat = stat
    member this.FormatStat() = $"stat: {this.stat}"
    member this.PrintStat() = this.FormatStat() |> printfn "%s"
    interface ICloneable with
        member this.Clone() =
            let dyn = this.GetPropertyValue("dyn") |> unbox<string>
            DerivedClassCloneable(stat, dyn)

let constructDeepCopiedClone<'T> (props: seq<string*obj>) =
    let original = DynamicObj()
    props
    |> Seq.iter (fun (propertyName, propertyValue) -> original.SetProperty(propertyName, propertyValue))
    let clone = original.DeepCopyProperties()
    original, clone |> unbox<'T>

let constructDeepCopiedObj<'T> (original: 'T) =
    original, (CopyUtils.tryDeepCopyObj original |> unbox<'T>)

let bulkMutate (props: seq<string*obj>) (dyn: #DynamicObj) =
    props |> Seq.iter (fun (propertyName, propertyValue) -> dyn.SetProperty(propertyName, propertyValue))


let firstDiff s1 s2 =
    let s1 = Seq.append (Seq.map Some s1) (Seq.initInfinite (fun _ -> None))
    let s2 = Seq.append (Seq.map Some s2) (Seq.initInfinite (fun _ -> None))
    Seq.mapi2 (fun i s p -> i,s,p) s1 s2
    |> Seq.find (function |_,Some s,Some p when s=p -> false |_-> true)

module DynObj =
    let inline getNestedPropAs<'T> (propTree: seq<string>) (dyn: DynamicObj) =
        let props = propTree |> Seq.toList
        let rec getProp (dyn: DynamicObj) (props: string list) : 'T=
            match props with
            | p::[] -> (dyn.GetPropertyValue(p)) |> unbox<'T>
            | p::ps -> getProp (dyn.GetPropertyValue(p) |> unbox<DynamicObj>) ps
            | _ -> failwith "Empty property list"
        getProp dyn props

module Expect =
    /// Expects the `actual` sequence to equal the `expected` one.
    let sequenceEqual actual expected message =
      match firstDiff actual expected with
      | _,None,None -> ()
      | i,Some a, Some e ->
        failwithf "%s. Sequence does not match at position %i. Expected item: %O, but got %O."
          message i e a
      | i,None,Some e ->
        failwithf "%s. Sequence actual shorter than expected, at pos %i for expected item %O."
          message i e
      | i,Some a,None ->
        failwithf "%s. Sequence actual longer than expected, at pos %i found item %O."
          message i a

    let referenceEqual actual expected message =
        if not (LanguagePrimitives.PhysicalEquality actual expected) then
            failwith message

    let notReferenceEqual actual expected message =
        if (LanguagePrimitives.PhysicalEquality actual expected) then
            failwith message