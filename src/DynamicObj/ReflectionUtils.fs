namespace DynamicObj


module ReflectionUtils =
    
    open System
    open System.Reflection
    
    // Gets public, static properties including interface propterties
    let getStaticProperties (o : obj) =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
            FableJS.getStaticPropertyHelpers o
        #endif
        #if FABLE_COMPILER_PYTHON
            FablePy.getStaticPropertyHelpers o
        #endif
        #if !FABLE_COMPILER
            let t = o.GetType()
            [| 
                for propInfo in t.GetProperties() -> propInfo
                for i in t.GetInterfaces() do yield! i.GetProperties()
            |]
            |> Array.map PropertyHelper.fromPropertyInfo
        #endif    

    /// Try to get the PropertyInfo by name using reflection
    let tryGetPropertyInfo (o:obj) (propName:string) =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        FableJS.getPropertyHelpers o
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.getPropertyHelpers o
        #endif
        #if !FABLE_COMPILER
        getStaticProperties (o)
        #endif
        |> Array.tryFind (fun n -> n.Name = propName)        

    let trySetPropertyValue (o:obj) (propName:string) (value:obj) =
        match tryGetPropertyInfo o propName with 
        | Some property when property.IsMutable ->
            property.SetValue o value
            true
        | _ -> false

    let tryGetPropertyValue (o:obj) (propName:string) =
        try 
            match tryGetPropertyInfo o propName with 
            | Some v -> Some (v.GetValue(o))
            | None -> None
        with 
        | :? System.Reflection.TargetInvocationException -> None
        | :? System.NullReferenceException -> None
    

    /// Gets property value as 'a option using reflection. Cast to 'a
    let tryGetPropertyValueAs<'a> (o:obj) (propName:string) =
        try 
            tryGetPropertyValue o propName
            |> Option.map (fun v -> v :?> 'a)
            
        with 
        | :? System.Reflection.TargetInvocationException -> None
        | :? System.NullReferenceException -> None

    let removeProperty (o:obj) (propName:string) =     

        match tryGetPropertyInfo o propName with         
        | Some property when property.IsMutable ->
            property.RemoveValue(o)
            true
        | _ -> false




    #if !FABLE_COMPILER
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

    /// Updates property value by given function
    let tryUpdatePropertyValueFromName (o:obj) (propName:string) (f: 'a -> 'a) =
        let v = optBuildApply f (tryGetPropertyValueAs<'a> o propName)
        trySetPropertyValue o propName v 
        //o

    /// Updates property value by given function
    let tryUpdatePropertyValue (o:obj) (expr : Microsoft.FSharp.Quotations.Expr) (f: 'a -> 'a) =
        let propName = tryGetPropertyName expr
        let g = (tryGetPropertyValueAs<'a> o propName.Value)
        let v = optBuildApply f g
        trySetPropertyValue o propName.Value v 
        //o

    let updatePropertyValueAndIgnore (o:obj) (expr : Microsoft.FSharp.Quotations.Expr) (f: 'a -> 'a) = 
        tryUpdatePropertyValue o expr f |> ignore

    #endif