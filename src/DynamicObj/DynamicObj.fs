namespace DynamicObj

//open System.Dynamic
open System.Collections.Generic
open Fable.Core

[<AttachMembers>]
type DynamicObj() = 
    
    //#if !FABLE_COMPILER
    //inherit DynamicObject()
    //#endif

    let mutable properties = new Dictionary<string, obj>()

    /// <summary>
    /// A dictionary of dynamic boxed properties
    /// </summary>
    member this.Properties
        with get() = properties
        and internal set(value) = properties <- value           

    /// <summary>
    /// Creates a new DynamicObj from a Dictionary containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties">The dictionary with the dynamic properties</param>
    static member ofDict (dynamicProperties: Dictionary<string,obj>) = 
        let obj = DynamicObj()
        obj.Properties <- dynamicProperties
        obj

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    member this.TryGetStaticPropertyHelper (propertyName: string) : PropertyHelper option  = 
        ReflectionUtils.tryGetStaticPropertyInfo this propertyName

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    member this.TryGetDynamicPropertyHelper (propertyName: string) : PropertyHelper option =
        #if FABLE_COMPILER_JAVASCRIPT  || FABLE_COMPILER_TYPESCRIPT
        FableJS.tryGetDynamicPropertyHelper this name
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.tryGetDynamicPropertyHelper this name
        #endif
        #if !FABLE_COMPILER
        match properties.TryGetValue propertyName with            
        | true,_ -> 
            Some {
                Name = propertyName
                IsStatic = false
                IsDynamic = true
                IsMutable = true
                IsImmutable = false
                GetValue = fun o -> properties.[propertyName]
                SetValue = fun o v -> properties.[propertyName] <- v
                RemoveValue = fun o -> properties.Remove(propertyName) |> ignore
            }
        | _      -> None
        #endif
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    member this.TryGetPropertyHelper (propertyName: string) : PropertyHelper option =
        match this.TryGetStaticPropertyHelper propertyName with
        | Some pi -> Some pi
        | None -> this.TryGetDynamicPropertyHelper propertyName

    /// <summary>
    /// Returns Some(boxed property value) if a dynamic (or static) property with the given name exists, otherwise None.
    /// </summary>
    /// <param name="name">the name of the property to get</param>
    member this.TryGetPropertyValue (propertyName: string) = 
        // first check the Properties collection for member
        this.TryGetPropertyHelper(propertyName)
        |> Option.map (fun pi -> pi.GetValue(this))
    
    /// <summary>
    /// Returns the boxed property value of the dynamic (or static) property with the given name.
    /// </summary>
    /// <param name="propertyName">the name of the property to get</param>
    /// <exception cref="System.MissingMemberException">Thrown if the dynamic property does not exist</exception>
    member this.GetPropertyValue (propertyName: string) =
        match this.TryGetPropertyValue(propertyName) with
        | Some value -> value
        | None -> raise <| System.MissingMemberException($"Property \"{propertyName}\" does not exist on object.")
        
    #if !FABLE_COMPILER
    
    /// <summary>
    /// Returns Some('TPropertyValue) when a dynamic (or static) property with the given name and type exists, otherwise None.
    ///
    /// This method is not Fable-compatible and can therefore not be used in code that will be transpiled.
    /// </summary>
    /// <param name="propertyName">the name of the property to get</param>
    /// <remarks>This method is not Fable-compatible and can therefore not be used in code that will be transpiled.</remarks>
    member this.TryGetTypedPropertyValue<'TPropertyValue> (propertyName: string) = 
        
        match (this.TryGetPropertyValue propertyName) with
        | None -> None
        | Some o ->      
            match o with
            | :? 'a as o -> o |> Some
            | _ -> None
    #endif    


    /// <summary>
    /// Sets the dynamic (or static) property value with the given name, creating a new dynamic property if none exists.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    member this.SetPropertyValue (
        propertyName: string,
        propertyValue: obj
    ) =
        // first check to see if there's a native property to set
        match this.TryGetStaticPropertyHelper propertyName with
        | Some pi ->
            if pi.IsMutable then
                pi.SetValue this propertyValue
            else
                failwith $"Cannot set value for static, immutable property \"{propertyName}\""
        | None -> 
            #if FABLE_COMPILER_JAVASCRIPT  || FABLE_COMPILER_TYPESCRIPT
            FableJS.setPropertyValue this name value
            #endif
            #if FABLE_COMPILER_PYTHON
            FablePy.setPropertyValue this name value
            #endif
            #if !FABLE_COMPILER
            // Next check the Properties collection for member
            match properties.TryGetValue propertyName with            
            | true,_ -> properties.[propertyName] <- propertyValue
            | _      -> properties.Add(propertyName,propertyValue)
            #endif

    /// <summary>
    /// Removes any dynamic property with the given name from the input DynamicObj.
    /// If the property is static and mutable, it will be set to null.
    /// Static immutable properties cannot be removed.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <exception cref="System.MemberAccessException">Thrown if the dynamic property does not exist</exception>
    member this.RemoveProperty (propertyName: string) =
        match this.TryGetPropertyHelper propertyName with
        | Some pi when pi.IsMutable -> 
            pi.RemoveValue this
            true
        | Some _ -> 
            raise <| System.MemberAccessException($"Cannot remove value for static, immutable property \"{propertyName}\"")
        | None -> false

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includeInstanceProperties"></param>
    member this.GetPropertyHelpers (includeInstanceProperties: bool) =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT           
        FableJS.getPropertyHelpers this
        |> Seq.filter (fun pd ->  
            includeInstanceProperties || pd.IsDynamic
        )
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.getPropertyHelpers this
        |> Seq.filter (fun pd ->  
            includeInstanceProperties || pd.IsDynamic
        )
        #endif
        #if !FABLE_COMPILER
        seq [
            if includeInstanceProperties then                
                yield! ReflectionUtils.getStaticProperties (this)
            for key in properties.Keys ->
                {
                    Name = key
                    IsStatic = false
                    IsDynamic = true
                    IsMutable = true
                    IsImmutable = false
                    GetValue = fun o -> properties.[key]
                    SetValue = fun o v -> properties.[key] <- v
                    RemoveValue = fun o -> properties.Remove(key) |> ignore
                }
        ]
        #endif
        |> Seq.filter (fun p -> p.Name.ToLower() <> "properties")

    /// Returns both instance and dynamic properties when passed true, only dynamic properties otherwise. 
    /// Properties are returned as a key value pair of the member names and the boxed values
    member this.GetProperties includeInstanceProperties : seq<KeyValuePair<string,obj>> =    
        this.GetPropertyHelpers(includeInstanceProperties)
        |> Seq.choose (fun kv -> 
            if kv.Name <> "properties" then
                Some (KeyValuePair(kv.Name, kv.GetValue this))
            else
                None
        )

    /// Copies all dynamic members of the DynamicObj to the target DynamicObj.
    member this.CopyDynamicPropertiesTo(target:#DynamicObj, ?overWrite) =
        let overWrite = Option.defaultValue false overWrite
        this.GetProperties(false)
        |> Seq.iter (fun kv ->
            match target.TryGetPropertyHelper kv.Key with
            | Some pi when overWrite -> pi.SetValue target kv.Value
            | Some _ -> failwith $"Property \"{kv.Key}\" already exists on target object and overWrite was not set to true."
            | None -> target.SetPropertyValue(kv.Key,kv.Value)
        )

    /// Returns a new DynamicObj with only the dynamic properties of the original DynamicObj (sans instance properties).
    member this.CopyDynamicProperties() =
        let target = DynamicObj()
        this.CopyDynamicPropertiesTo(target)
        target

    member this.GetPropertyNames(includeInstanceProperties) =
        this.GetProperties(includeInstanceProperties)
        |> Seq.map (fun kv -> kv.Key)


    /// <summary>
    /// Operator to access a dynamic member by name
    /// </summary>
    /// <remarks>This operator is not Fable-compatible</remarks>
    static member (?) (lookup:#DynamicObj,name:string) =
        match lookup.TryGetPropertyValue name with
        | Some(value) -> value
        | None -> raise <| System.MemberAccessException()        

    /// <summary>
    /// Operator to set a dynamic member
    /// </summary>
    /// <remarks>This operator is not Fable-compatible</remarks>
    static member (?<-) (lookup:#DynamicObj,name:string,value:'v) =
        lookup.SetPropertyValue (name,value)

    override this.GetHashCode () =
        this.GetProperties(true)
        |> Seq.sortBy (fun pair -> pair.Key)
        |> HashCodes.boxHashKeyValSeq
        |> fun x -> x :?> int

    override this.Equals o =
        match o with
        | :? DynamicObj as other ->
            this.GetHashCode() = other.GetHashCode()
        | _ -> false
