namespace DynamicObj

open Fable.Core

module ReflectionUtils =
    
    open System
    open System.Reflection
    
    #if !FABLE_COMPILER

    // Gets public properties including interface propterties
    let getPublicProperties (t:Type) =
        [|
            for propInfo in t.GetProperties() -> propInfo
            for i in t.GetInterfaces() do yield! i.GetProperties()
        |]

    /// Creates an instance of the Object according to applyStyle and applies the function..
    let buildApply (applyStyle:'a -> 'a) =
        let instance =
            System.Activator.CreateInstance<'a>()
        applyStyle instance

    /// Applies 'applyStyle' to item option. If None it creates a new instance.
    let optBuildApply (applyStyle:'a -> 'a) (item:'a option) =
        match item with
        | Some item' -> applyStyle item'
        | None       -> buildApply applyStyle

    /// Applies Some 'applyStyle' to item. If None it returns 'item' unchanged.
    let optApply (applyStyle:('a -> 'a)  option) (item:'a ) =
        match applyStyle with
        | Some apply -> apply item
        | None       -> item

    /// Returns the proptery name from quotation expression
    let tryGetPropertyName (expr : Microsoft.FSharp.Quotations.Expr) =
        match expr with
        | Microsoft.FSharp.Quotations.Patterns.PropertyGet (_,pInfo,_) -> Some pInfo.Name
        | _ -> None

    /// Try to get the PropertyInfo by name using reflection
    let tryGetPropertyInfo (o:obj) (propName:string) =
        getPublicProperties (o.GetType())
        |> Array.tryFind (fun n -> n.Name = propName)        

    #endif

    /// Sets property value using reflection
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    [<Emit("$0[$1] = $2")>]
    #endif
    let trySetPropertyValue (o:obj) (propName:string) (value:obj) =
        
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        jsNative
        #else

        match tryGetPropertyInfo o propName with 
        | Some property ->
            try 
                property.SetValue(o, value, null)
                Some o
            with
            | :? System.ArgumentException -> None
            | :? System.NullReferenceException -> None
        | None -> None
        #endif

    /// Gets property value as option using reflection
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    [<Emit("$0[$1]")>]
    #endif
    let tryGetPropertyValue (o:obj) (propName:string) =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        jsNative
        #else
        try 
            match tryGetPropertyInfo o propName with 
            | Some v -> Some (v.GetValue(o,null))
            | None -> None
        with 
        | :? System.Reflection.TargetInvocationException -> None
        | :? System.NullReferenceException -> None
        #endif
    
    ///// Gets property value as 'a option using reflection. Cast to 'a
    //let tryGetPropertyValueAs<'a> (o:obj) (propName:string) =
    //    try 
    //        tryGetPropertyValue o propName
    //        |> Option.map (fun v -> v :?> 'a)
            
    //    with 
    //    | :? System.Reflection.TargetInvocationException -> None
    //    | :? System.NullReferenceException -> None

    ///// Updates property value by given function
    //let tryUpdatePropertyValueFromName (o:obj) (propName:string) (f: 'a -> 'a) =
    //    let v = optBuildApply f (tryGetPropertyValueAs<'a> o propName)
    //    trySetPropertyValue o propName v 
    //    //o

    ///// Updates property value by given function
    //let tryUpdatePropertyValue (o:obj) (expr : Microsoft.FSharp.Quotations.Expr) (f: 'a -> 'a) =
    //    let propName = tryGetPropertyName expr
    //    let g = (tryGetPropertyValueAs<'a> o propName.Value)
    //    let v = optBuildApply f g
    //    trySetPropertyValue o propName.Value v 
    //    //o

    //let updatePropertyValueAndIgnore (o:obj) (expr : Microsoft.FSharp.Quotations.Expr) (f: 'a -> 'a) = 
    //    tryUpdatePropertyValue o expr f |> ignore


    /// Sets property value using reflection
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    [<Emit("$0[$1] = null")>]
    #endif
    let removeProperty (o:obj) (propName:string) =     
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        jsNative
        #else
        match tryGetPropertyInfo o propName with         
        | Some property ->
            try 
                property.SetValue(o, null, null)
                true
            with
            | :? System.ArgumentException -> false
            | :? System.NullReferenceException -> false
        | None -> false
        #endif
