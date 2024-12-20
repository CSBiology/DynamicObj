﻿namespace DynamicObj

#if !FABLE_COMPILER
open System.Dynamic
#endif

open System.Collections.Generic
open Fable.Core

[<AttachMembers>]
type DynamicObj() =
    
    #if !FABLE_COMPILER
    inherit DynamicObject()
    #endif
    
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
    /// Returns Some(PropertyHelper) if a static property with the given name exists, otherwise None.
    /// </summary>
    /// <param name="propertyName">The name of the property to get the PropertyHelper for</param>
    member this.TryGetStaticPropertyHelper (propertyName: string) : PropertyHelper option  = 
        ReflectionUtils.tryGetStaticPropertyInfo this propertyName

    /// <summary>
    /// Returns Some(PropertyHelper) if a dynamic property with the given name exists, otherwise None.
    /// </summary>
    /// <param name="propertyName">The name of the property to get the PropertyHelper for</param>
    member this.TryGetDynamicPropertyHelper (propertyName: string) : PropertyHelper option =
        #if FABLE_COMPILER_JAVASCRIPT  || FABLE_COMPILER_TYPESCRIPT
        FableJS.tryGetDynamicPropertyHelper this propertyName
        #endif
        #if FABLE_COMPILER_PYTHON
        FablePy.tryGetDynamicPropertyHelper this propertyName
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
    /// Returns Some(PropertyHelper) if a property (static or dynamic) with the given name exists, otherwise None.
    /// </summary>
    /// <param name="propertyName">The name of the property to get the PropertyHelper for</param>
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
    /// <exception cref="System.MissingMemberException">Thrown if no dynamic or static property with the given name exists</exception>
    member this.GetPropertyValue (propertyName: string) =
        match this.TryGetPropertyValue(propertyName) with
        | Some value -> value
        | None -> raise <| System.MissingMemberException($"No dynamic or static property \"{propertyName}\" does exist on object.")
        
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
            | :? 'TPropertyValue as o -> o |> Some
            | _ -> None
    #endif    


    /// <summary>
    /// Sets the dynamic (or static) property value with the given name, creating a new dynamic property if none exists.
    /// </summary>
    /// <param name="propertyName">the name of the property to set</param>
    /// <param name="propertyValue">the value of the property to set</param>
    member this.SetProperty (
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
            FableJS.setPropertyValue this propertyName propertyValue
            #endif
            #if FABLE_COMPILER_PYTHON
            FablePy.setPropertyValue this propertyName propertyValue
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
    /// <param name="propertyName">the name of the property to remove</param>
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
    /// Returns PropertyHelpers for all dynamic properties of the DynamicObj.
    ///
    /// When includeInstanceProperties is set to true, instance properties (= 'static' properties on the class) are included in the result.
    /// </summary>
    /// <param name="includeInstanceProperties">whether to include instance properties (= 'static' properties on the class)</param>
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

    /// <summary>
    /// Returns a sequence of all dynamic properties as a key value pair of the property names and the boxed property values.
    ///
    /// When includeInstanceProperties is set to true, instance properties (= 'static' properties on the class) are included in the result.
    /// </summary>
    /// <param name="includeInstanceProperties">whether to include instance properties (= 'static' properties on the class)</param>
    member this.GetProperties includeInstanceProperties : seq<KeyValuePair<string,obj>> =    
        this.GetPropertyHelpers(includeInstanceProperties)
        |> Seq.choose (fun kv -> 
            if kv.Name <> "properties" then
                Some (KeyValuePair(kv.Name, kv.GetValue this))
            else
                None
        )

    /// <summary>
    /// Returns a sequence of all dynamic property names.
    ///
    /// When includeInstanceProperties is set to true, instance properties (= 'static' properties on the class) are included in the result.
    /// </summary>
    /// <param name="includeInstanceProperties"></param>
    member this.GetPropertyNames(includeInstanceProperties) =
        this.GetProperties(includeInstanceProperties)
        |> Seq.map (fun kv -> kv.Key)

    /// <summary>
    /// Copies all dynamic properties to a target `DynamicObj` instance without trying to prevent reference equality.
    ///
    /// Note that this function does not attempt to do any deep copying. 
    /// The dynamic properties of the source will be copied as references to the target. 
    /// If any of those properties are mutable or themselves DynamicObj instances, changes to the properties on the source will be reflected in the target.
    ///
    /// If overWrite is set to true, existing properties on the target object will be overwritten.
    /// </summary>
    /// <param name="target">The target object to copy dynamic members to</param>
    /// <param name="overWrite">Whether existing properties on the target object will be overwritten</param>
    member this.ShallowCopyDynamicPropertiesTo(target:#DynamicObj, ?overWrite) =
        let overWrite = defaultArg overWrite false 
        this.GetProperties(false)
        |> Seq.iter (fun kv ->
            match target.TryGetPropertyHelper kv.Key with
            | Some pi when overWrite -> pi.SetValue target kv.Value
            | Some _ -> ()
            | None -> target.SetProperty(kv.Key,kv.Value)
        )

    /// <summary>
    /// Copies all dynamic properties to a new `DynamicObj` instance without trying to prevent reference equality.
    ///
    /// Note that this function does not attempt to do any deep copying. 
    /// The dynamic properties of the source will be copied as references to the target. 
    /// If any of those properties are mutable or themselves DynamicObj instances, changes to the properties on the source will be reflected in the target.
    /// </summary>
    member this.ShallowCopyDynamicProperties() =
        let target = DynamicObj()
        this.ShallowCopyDynamicPropertiesTo(target, true)
        target

    /// <summary>
    /// Recursively deep copies **all** (static and dynamic) properties to a **target** `DynamicObj` instance (or derived class). Reinstantiation - and therefore prevention of reference equality - is possible for `DynamicObj`, `array|list|ResizeArray&lt;DynamicObj&gt;`, and classes implementing `System.Icloneable`
    /// 
    /// As many properties as possible are re-instantiated as new objects, meaning the 
    /// copy has as little reference equal properties as possible.
    /// 
    /// The nature of DynamicObj however means that it is impossible to reliably deep copy all properties, as
    /// their type is not known on runtime and the contructors of the types are not known.
    ///
    /// The following cases are handled (in this precedence):
    ///
    /// - Basic F# types (int, float, bool, string, char, byte, sbyte, int16, uint16, int32, uint32, int64, uint64, single, decimal)
    ///
    /// - array&lt;DynamicObj&gt;, list&lt;DynamicObj&gt;, ResizeArray&lt;DynamicObj&gt;: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
    ///
    /// - System.ICloneable: If the property implements ICloneable, the Clone() method is called on the property.
    ///
    /// - DynamicObj (and derived classes): properties that are themselves DynamicObj instances are deep copied recursively.
    ///   if a derived class has static properties (e.g. instance properties), these will be copied as dynamic properties on the new instance.
    ///
    /// Note on Classes that inherit from DynamicObj:
    ///
    /// Classes that inherit from DynamicObj will match the `DynamicObj` typecheck if they do not implement ICloneable.
    /// The deep coopied instances will be cast to DynamicObj with static/instance properties AND dynamic properties all set as dynamic properties.
    /// It should be possible to 'recover' the original type by checking if the needed properties exist as dynamic properties,
    /// and then passing them to the class constructor if needed.
    /// </summary>
    /// <param name="target">The target object to copy dynamic members to</param>
    /// <param name="overWrite">Whether existing properties on the target object will be overwritten</param>
    member this.DeepCopyPropertiesTo(target:#DynamicObj, ?overWrite) =
        let overWrite = defaultArg overWrite false

        this.GetProperties(true)
        |> Seq.iter (fun kv ->
            match target.TryGetPropertyHelper kv.Key with
            | Some pi when overWrite -> pi.SetValue target (CopyUtils.tryDeepCopyObj kv.Value)
            | Some _ -> ()
            | None -> target.SetProperty(kv.Key, CopyUtils.tryDeepCopyObj kv.Value)
        )

    /// <summary>
    /// Recursively deep copy a `DynamicObj` instance (or derived class) with **all** (static and dynamic) properties. Reinstantiation - and therefore prevention of reference equality - is possible for `DynamicObj`, `array|list|ResizeArray&lt;DynamicObj&gt;`, and classes implementing `System.Icloneable`
    /// 
    /// On the deep copy, as many properties as possible are re-instantiated as new objects, meaning the 
    /// copy has as little reference equal properties as possible.
    /// 
    /// The nature of DynamicObj however means that it is impossible to reliably deep copy all properties, as
    /// their type is not known on runtime and the contructors of the types are not known.
    ///
    /// The following cases are handled (in this precedence):
    ///
    /// - Basic F# types (int, float, bool, string, char, byte, sbyte, int16, uint16, int32, uint32, int64, uint64, single, decimal)
    ///
    /// - array&lt;DynamicObj&gt;, list&lt;DynamicObj&gt;, ResizeArray&lt;DynamicObj&gt;: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
    ///
    /// - System.ICloneable: If the property implements ICloneable, the Clone() method is called on the property.
    ///
    /// - DynamicObj (and derived classes): properties that are themselves DynamicObj instances are deep copied recursively.
    ///   if a derived class has static properties (e.g. instance properties), these will be copied as dynamic properties on the new instance.
    ///
    /// Note on Classes that inherit from DynamicObj:
    ///
    /// Classes that inherit from DynamicObj will match the `DynamicObj` typecheck if they do not implement ICloneable.
    /// The deep coopied instances will be cast to DynamicObj with static/instance properties AND dynamic properties all set as dynamic properties.
    /// It should be possible to 'recover' the original type by checking if the needed properties exist as dynamic properties,
    /// and then passing them to the class constructor if needed.
    /// </summary>
    /// <param name="target">The target object to copy dynamic members to</param>
    /// <param name="overWrite">Whether existing properties on the target object will be overwritten</param>
    member this.DeepCopyProperties() = CopyUtils.tryDeepCopyObj this

    #if !FABLE_COMPILER
    // Some necessary overrides for methods inherited from System.Dynamic.DynamicObject()
    // 
    // Needed mainly for making Newtonsoft.Json Serialization work
    override this.TryGetMember(binder:GetMemberBinder,result:obj byref ) =     
        match this.TryGetPropertyValue binder.Name with
        | Some value -> result <- value; true
        | None -> false

    override this.TrySetMember(binder:SetMemberBinder, value:obj) =        
        this.SetProperty(binder.Name,value)
        true

    /// Returns both instance and dynamic member names.
    /// Important to return both so JSON serialization with Json.NET works.
    override this.GetDynamicMemberNames() = this.GetPropertyNames(true)

    //// potential deserialization support
    //[<JsonExtensionData>]
    //member private this._additionalData : IDictionary<string, JToken> = new Dictionary<string, JToken>()

    //[<OnDeserialized>]
    //member private this.OnDeserialized(context:StreamingContext) = ()
    //    map over key value pairs in additional data, box the token values and set dynamic properties via SetProperty.

    #endif

    /// <summary>
    /// Operator to access a property by name
    ///
    /// This method is not Fable-compatible and can therefore not be used in code that will be transpiled.
    /// </summary>
    /// <remarks>This method is not Fable-compatible and can therefore not be used in code that will be transpiled.</remarks>
    static member (?) (lookup:#DynamicObj,name:string) =
        match lookup.TryGetPropertyValue name with
        | Some(value) -> value
        | None -> raise <| System.MemberAccessException()        

    /// <summary>
    /// Operator to set a property value
    ///
    /// This method is not Fable-compatible and can therefore not be used in code that will be transpiled.
    /// </summary>
    /// <remarks>This method is not Fable-compatible and can therefore not be used in code that will be transpiled.</remarks>
    static member (?<-) (lookup:#DynamicObj,name:string,value:'v) =
        lookup.SetProperty (name,value)

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

and CopyUtils =

    /// internal helper function to deep copy a boxed object (if possible)
    static member tryDeepCopyObj (o:obj) =
        let rec tryDeepCopyObj (o:obj) =
            match o with

            // might be that we do not need this case, however if we remove it, some types will match the 
            // ICloneable case in transpiled code, which we'd like to prevent, so well keep it for now.
            | :? int     | :? float   | :? bool    
            | :? string  | :? char    | :? byte    
            | :? sbyte   | :? int16   | :? uint16  
            | :? int32   | :? uint32  | :? int64   
            | :? uint64  | :? single  
                -> o

            #if !FABLE_COMPILER_PYTHON
            // https://github.com/fable-compiler/Fable/issues/3971
            | :? decimal -> o
            #endif

            | :? array<DynamicObj> as dyns ->
                box [|for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj|]
            | :? list<DynamicObj> as dyns ->
                box [for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj]
            | :? ResizeArray<DynamicObj> as dyns ->
                box (ResizeArray([for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj]))
            #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
            | o when FableJS.Interfaces.implementsICloneable o -> FableJS.Interfaces.cloneICloneable o
            #endif
            #if FABLE_COMPILER_PYTHON
            // https://github.com/fable-compiler/Fable/issues/3972
            | o when FablePy.Interfaces.implementsICloneable o -> FablePy.Interfaces.cloneICloneable o
            #endif
            #if !FABLE_COMPILER
            | :? System.ICloneable as clonable -> clonable.Clone()
            #endif

            | :? DynamicObj as dyn ->
                let newDyn = DynamicObj()
                // might want to keep instance props as dynamic props on copy
                for kv in (dyn.GetProperties(true)) do
                    newDyn.SetProperty(kv.Key, tryDeepCopyObj kv.Value)
                box newDyn
            | _ -> o

        tryDeepCopyObj o