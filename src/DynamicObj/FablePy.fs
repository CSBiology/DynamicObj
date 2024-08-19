namespace DynamicObj


open Fable.Core
open System.Collections.Generic

module FablePy =
    
    type PropertyObject = 
        abstract fget : obj
        abstract fset : obj

    module PropertyObject = 
        
        [<Emit("$0.fget")>]
        let tryGetGetter (o:PropertyObject) : obj option =
            nativeOnly

        [<Emit("$0.fset")>]
        let tryGetSetter (o:PropertyObject) : obj option =
            nativeOnly

        let containsGetter (o:obj) : bool =
            match tryGetGetter o with
            | Some _ -> true
            | None -> false

        let containsSetter (o:obj) : bool =
            match tryGetSetter o with
            | Some _ -> true
            | None -> false

        let isWritable (o:obj) : bool =
            containsSetter o

        [<Emit("isinstance($0, property)")>]
        let isProperty (o:obj) : bool =
            nativeOnly

    [<Emit("vars($0)")>]
    let getOwnMemberObjects (o:obj) : Dictionary<string,obj> =
        nativeOnly

    [<Emit("$0.__class__")>]
    let getClass (o:obj) : obj =
        nativeOnly

    let getOwnPropertyObjects (o:obj) : Dictionary<string,obj> =
        getOwnMemberObjects o



    let getStaticPropertyObjects (o:obj) =
        getClass o
        |> getOwnPropertyNames
        |> Array.filter (fun n -> n <> "constructor")

    [<Emit("$0.$1 = $2")>]
    let setPropertyValue (o:obj) (propName:string) (value:obj) =    
        nativeOnly

    let createSetter (propName:string) =
        fun (o:obj) (value:obj) -> 
         setPropertyValue o propName value

    let removeStaticPropertyValue (o:obj) (propName:string) =
        setPropertyValue o propName null

    [<Emit("delete $0[$1]")>]
    let deleteDynamicPropertyValue (o:obj) (propName:string) =
        nativeOnly

    let createRemover (propName:string) (isStatic : bool) =
        if isStatic then
            fun (o:obj) -> 
                removeStaticPropertyValue o propName
        else
            fun (o:obj) -> 
                deleteDynamicPropertyValue o propName


    [<Emit("$0[$1]")>]
    let getPropertyValue (o:obj) (propName:string) =
        nativeOnly

    let createGetter (propName:string) =
        fun (o:obj) -> 
            getPropertyValue o propName

    [<Emit("Object.getOwnPropertyDescriptor($0, $1)")>]
    let getPropertyDescriptor (o:obj) (propName:string) =
        nativeOnly

    let getStaticPropertyDescriptor (o:obj) (propName:string) =
        getPropertyDescriptor (getPrototype o) propName

    let getStaticPropertyHelpers (o:obj) : PropertyHelper [] =
        getStaticPropertyNames o
        |> Array.choose (fun n ->
            let pd = getStaticPropertyDescriptor o n
            if PropertyDescriptor.isFunction pd then 
                None
            else 
                let isWritable = PropertyDescriptor.isWritable pd
                {
                    Name = n
                    IsStatic = true
                    IsDynamic = false
                    IsMutable = isWritable
                    IsImmutable = not isWritable
                    GetValue = createGetter n
                    SetValue = createSetter n
                    RemoveValue = createRemover n true
                }     
                |> Some
        )

    let transpiledPropertyRegex = "^[a-zA-Z]+@[0-9]+$"

    let isTranspiledPropertyHelper (propertyName : string) =
        System.Text.RegularExpressions.Regex.IsMatch(propertyName, transpiledPropertyRegex)

    let getDynamicPropertyHelpers (o:obj) : PropertyHelper [] =
        getOwnPropertyNames o
        |> Array.choose (fun n ->
            let pd = getPropertyDescriptor o n
            if PropertyDescriptor.isFunction pd || isTranspiledPropertyHelper n then 
                None
            else 
                let isWritable = PropertyDescriptor.isWritable pd
                {
                    Name = n
                    IsStatic = false
                    IsDynamic = true
                    IsMutable = isWritable
                    IsImmutable = not isWritable
                    GetValue = createGetter n
                    SetValue = createSetter n
                    RemoveValue = createRemover n false
                }        
                |> Some
        )

    let getPropertyHelpers (o:obj) =
        getDynamicPropertyHelpers o
        |> Array.append (getStaticPropertyHelpers o)

    let getPropertyNames (o:obj) =
        getPropertyHelpers o 
        |> Array.map (fun h -> h.Name)

