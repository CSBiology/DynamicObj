namespace DynamicObj


open Fable.Core
open System.Collections.Generic

module FablePy =
    
    module Dictionary = 
        
        let ofSeq (s:seq<KeyValuePair<_,_>>) =
            let d = new System.Collections.Generic.Dictionary<_,_>()
            s |> Seq.iter (fun kv -> d.Add(kv.Key, kv.Value))
            d

        let choose (f: 'T -> 'U option) (d:System.Collections.Generic.Dictionary<_,'T>) =
            let nd = new System.Collections.Generic.Dictionary<_,'U>()
            for kv in d do
                match f kv.Value with
                | Some v -> nd.Add(kv.Key, v)
                | None -> ()
            nd

    type PropertyObject = 
        abstract fget : obj
        abstract fset : obj

    module PropertyObject = 
        
        [<Emit("$0.fget")>]
        let tryGetGetter (o:PropertyObject) : (obj -> obj) option =
            nativeOnly

        [<Emit("$0.fset")>]
        let tryGetSetter (o:PropertyObject) : (obj -> obj -> unit) option =
            nativeOnly

        let getGetter (o : PropertyObject) : obj -> obj =
            match tryGetGetter o with
            | Some f -> f
            | None -> fun o -> failwith ("Property does not contain getter")

        let getSetter (o:PropertyObject) : obj -> obj -> unit =
            match tryGetSetter o with
            | Some f -> f
            | None -> fun s o -> failwith ("Property does not contain setter")

        let containsGetter (o:PropertyObject) : bool =
            match tryGetGetter o with
            | Some _ -> true
            | None -> false

        let containsSetter (o:PropertyObject) : bool =
            match tryGetSetter o with
            | Some _ -> true
            | None -> false

        let isWritable (o:PropertyObject) : bool =
            containsSetter o

        [<Emit("isinstance($0, property)")>]
        let isProperty (o:obj) : bool =
            nativeOnly

        let tryProperty (o:obj) : PropertyObject option =
            if isProperty o then
                Some (o :?> PropertyObject)
            else
                None

    [<Emit("getattr($0,$1)")>]
    let getPropertyValue (o:obj) (propName:string) =
        nativeOnly

    let createGetter (propName:string) =
        fun (o:obj) -> 
            getPropertyValue o propName

    [<Emit("setattr($0,$1,$2)")>]
    let setPropertyValue (o:obj) (propName:string) (value:obj) : unit =    
        nativeOnly

    let createSetter (propName:string) =
        fun (o:obj) (value:obj) -> 
         setPropertyValue o propName value


    [<Emit("vars($0).items()")>]
    let getOwnMemberObjects (o:obj) : Dictionary<string,obj> =
        nativeOnly

    [<Emit("$0.__class__")>]
    let getClass (o:obj) : obj =
        nativeOnly

    let getStaticPropertyObjects (o:obj) : Dictionary<string,PropertyObject> =
        getClass o
        |> getOwnMemberObjects
        |> Dictionary.choose PropertyObject.tryProperty

    let removeStaticPropertyValue (o:obj) (propName:string) =
        setPropertyValue o propName null

    [<Emit("delattr($0,$1)")>]
    let deleteDynamicPropertyValue (o:obj) (propName:string) =
        nativeOnly

    let createRemover (propName:string) (isStatic : bool) =
        if isStatic then
            fun (o:obj) -> 
                removeStaticPropertyValue o propName
        else
            fun (o:obj) -> 
                deleteDynamicPropertyValue o propName



    [<Emit("$0.__dict__.get($1)")>]
    let getMemberObject (o:obj) (propName:string) =
        nativeOnly

    let tryGetPropertyObject (o:obj) (propName:string) : PropertyObject option =
        match PropertyObject.tryProperty (getMemberObject o propName) with
        | Some po -> Some po
        | None -> None

    let tryGetDynamicPropertyHelper (o:obj) (propName:string) : PropertyHelper option =
        match getMemberObject o propName with
        | Some _ -> 
            Some {
                Name = propName
                IsStatic = false
                IsDynamic = true
                IsMutable = true
                IsImmutable = false
                GetValue = createGetter propName
                SetValue = createSetter propName
                RemoveValue = fun o -> deleteDynamicPropertyValue o propName
            }
        | None -> None

    let tryGetStaticPropertyHelper (o:obj) (propName:string) : PropertyHelper option =
        match tryGetPropertyObject (getClass o) propName with
        | Some po -> 
            let isWritable = PropertyObject.isWritable po
            Some {
                Name = propName
                IsStatic = true
                IsDynamic = false
                IsMutable = isWritable
                IsImmutable = not isWritable
                GetValue = createGetter propName
                SetValue = createSetter propName
                RemoveValue = fun o -> removeStaticPropertyValue o propName
            }
         | None -> None

    let transpiledPropertyRegex = "^([a-zA-Z]+_)+[0-9]+$"

    let isTranspiledPropertyHelper (propertyName : string) =
        System.Text.RegularExpressions.Regex.IsMatch(propertyName, transpiledPropertyRegex)


    let getDynamicPropertyHelpers (o:obj) : PropertyHelper [] =
        getOwnMemberObjects o
        |> Seq.choose (fun kv -> 
            let n = kv.Key
            if isTranspiledPropertyHelper n then 
                None
            else
                {
                    Name = n
                    IsStatic = false
                    IsDynamic = true
                    IsMutable = true
                    IsImmutable = false
                    GetValue = createGetter n
                    SetValue = createSetter n
                    RemoveValue = fun o -> deleteDynamicPropertyValue o n
                }
                |> Some
        )
        |> Seq.toArray


    let getStaticPropertyHelpers (o:obj) : PropertyHelper [] =
        getStaticPropertyObjects o
        |> Seq.map (fun kv -> 
            let n = kv.Key
            let po = kv.Value
            {
                Name = n
                IsStatic = true
                IsDynamic = false
                IsMutable = PropertyObject.isWritable po
                IsImmutable = not (PropertyObject.isWritable po)
                GetValue = createGetter n
                SetValue = createSetter n
                RemoveValue = fun o -> removeStaticPropertyValue o n
            }
        )
        |> Seq.toArray

    let getPropertyHelpers (o:obj) =
        getDynamicPropertyHelpers o
        |> Array.append (getStaticPropertyHelpers o)

    let getPropertyNames (o:obj) =
        getPropertyHelpers o 
        |> Array.map (fun h -> h.Name)

