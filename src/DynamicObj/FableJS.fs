namespace DynamicObj


open Fable.Core

module FableJS =
    
    module PropertyDescriptor = 
        
        [<Emit("$0[$1]")>]
        let tryGetPropertyValue (o:obj) (propName:string) : obj option =
            jsNative

        let tryGetIsWritable (o:obj) : bool option =
            tryGetPropertyValue o "writable"
            |> Option.map (fun v -> v :?> bool)

        let containsGetter (o:obj) : bool =
            match tryGetPropertyValue o "get" with
            | Some _ -> true
            | None -> false

        let containsSetter (o:obj) : bool =
            match tryGetPropertyValue o "set" with
            | Some _ -> true
            | None -> false

        let isWritable (o:obj) : bool =
            match tryGetIsWritable o with
            | Some v -> v
            | None -> containsSetter o

        [<Emit("typeof $0 === 'function'")>]
        let valueIsFunction (o:obj) : bool =
            jsNative

        let isFunction (o:obj) : bool =
            match tryGetPropertyValue o "value" with
            | Some v -> valueIsFunction v
            | None -> false

    [<Emit("Object.getOwnPropertyNames($0)")>]
    let getOwnPropertyNames (o:obj) : string [] =
        jsNative

    [<Emit("Object.getPrototypeOf($0)")>]
    let getPrototype (o:obj) : obj =
        jsNative

    let getStaticPropertyNames (o:obj) =
        getPrototype o
        |> getOwnPropertyNames
        |> Array.filter (fun n -> n <> "constructor")

    [<Emit("$0[$1] = $2")>]
    let setPropertyValue (o:obj) (propName:string) (value:obj) =    
        jsNative

    let createSetter (propName:string) =
        fun (o:obj) (value:obj) -> 
         setPropertyValue o propName value

    let removeStaticPropertyValue (o:obj) (propName:string) =
        setPropertyValue o propName null

    [<Emit("delete $0[$1]")>]
    let deleteDynamicPropertyValue (o:obj) (propName:string) =
        jsNative

    let createRemover (propName:string) (isStatic : bool) =
        if isStatic then
            fun (o:obj) -> 
                removeStaticPropertyValue o propName
        else
            fun (o:obj) -> 
                deleteDynamicPropertyValue o propName


    [<Emit("$0[$1]")>]
    let getPropertyValue (o:obj) (propName:string) =
        jsNative

    let createGetter (propName:string) =
        fun (o:obj) -> 
            getPropertyValue o propName

    [<Emit("Object.getOwnPropertyDescriptor($0, $1)")>]
    let getPropertyDescriptor (o:obj) (propName:string) =
        jsNative

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

