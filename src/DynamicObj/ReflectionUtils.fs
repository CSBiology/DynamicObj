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
    let tryGetStaticPropertyInfo (o:obj) (propName:string) =      
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        FableJS.tryGetStaticPropertyHelper o propName
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.tryGetStaticPropertyHelper o propName
        #endif
        #if !FABLE_COMPILER
        getStaticProperties (o)        
        |> Array.tryFind (fun n -> n.Name = propName)     
        #endif

    let trySetPropertyValue (o:obj) (propName:string) (value:obj) =
        match tryGetStaticPropertyInfo o propName with 
        | Some property when property.IsMutable ->
            property.SetValue o value
            true
        | _ -> false

    let tryGetPropertyValue (o:obj) (propName:string) =
        try 
            match tryGetStaticPropertyInfo o propName with 
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

        match tryGetStaticPropertyInfo o propName with         
        | Some property when property.IsMutable ->
            property.RemoveValue(o)
            true
        | _ -> false
