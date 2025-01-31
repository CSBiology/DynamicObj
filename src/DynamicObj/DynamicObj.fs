namespace DynamicObj

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
        and set(value) = properties <- value           

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
    /// - Basic F# types (`bool`, `byte`, `sbyte`, `int16`, `uint16`, `int`, `uint`, `int64`, `uint64`, `nativeint`, `unativeint`, `float`, `float32`, `char`, `string`, `unit`, `decimal`)
    /// 
    /// - `ResizeArrays` and `Dictionaries` containing any combination of basic F# types
    /// 
    /// - `Dictionaries` containing `DynamicObj` as keys or values in any combination with `DynamicObj` or basic F# types as keys or values
    /// 
    /// - `array&lt;DynamicObj&gt;`, `list&lt;DynamicObj&gt;`, `ResizeArray&lt;DynamicObj&gt;`: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
    /// 
    /// - `System.ICloneable`: If the property implements `ICloneable`, the `Clone()` method is called on the property.
    /// 
    /// - `DynamicObj` (and derived classes): properties that are themselves `DynamicObj` instances are deep copied recursively.
    ///   if a derived class has static properties (e.g. instance properties), these can be copied as dynamic properties on the new instance or ignored.
    /// 
    /// Note on Classes that inherit from `DynamicObj`:
    /// 
    /// Classes that inherit from DynamicObj will match the `DynamicObj` typecheck if they do not implement `ICloneable`.
    /// The deep copied instances will be cast to `DynamicObj` with deep copied dynamic properties. Staic/instance properties can be copied as dynamic properties on the new instance or be ignored.
    /// It should be possible to 'recover' the original type by checking if the needed properties exist as dynamic properties,
    /// and then passing them to the class constructor if needed.
    /// </summary>
    /// <param name="target">The target object to copy dynamic members to</param>
    /// <param name="overWrite">Whether existing properties on the target object will be overwritten. Default is false</param>
    /// <param name="includeInstanceProperties">Whether to include instance properties (= 'static' properties on the class) as dynamic properties on the new instance. Default is true</param>
    member this.DeepCopyPropertiesTo(
        target:#DynamicObj, 
        ?overWrite: bool, 
        ?includeInstanceProperties:bool
    ) =
        let overWrite = defaultArg overWrite false
        let includeInstanceProperties = defaultArg includeInstanceProperties true
        this.GetProperties(includeInstanceProperties)
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
    /// - Basic F# types (`bool`, `byte`, `sbyte`, `int16`, `uint16`, `int`, `uint`, `int64`, `uint64`, `nativeint`, `unativeint`, `float`, `float32`, `char`, `string`, `unit`, `decimal`)
    /// 
    /// - `ResizeArrays` and `Dictionaries` containing any combination of basic F# types
    /// 
    /// - `Dictionaries` containing `DynamicObj` as keys or values in any combination with `DynamicObj` or basic F# types as keys or values
    /// 
    /// - `array&lt;DynamicObj&gt;`, `list&lt;DynamicObj&gt;`, `ResizeArray&lt;DynamicObj&gt;`: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
    /// 
    /// - `System.ICloneable`: If the property implements `ICloneable`, the `Clone()` method is called on the property.
    /// 
    /// - `DynamicObj` (and derived classes): properties that are themselves `DynamicObj` instances are deep copied recursively.
    ///   if a derived class has static properties (e.g. instance properties), these can be copied as dynamic properties on the new instance or ignored.
    /// 
    /// Note on Classes that inherit from `DynamicObj`:
    /// 
    /// Classes that inherit from DynamicObj will match the `DynamicObj` typecheck if they do not implement `ICloneable`.
    /// The deep copied instances will be cast to `DynamicObj` with deep copied dynamic properties. Staic/instance properties can be copied as dynamic properties on the new instance or be ignored.
    /// It should be possible to 'recover' the original type by checking if the needed properties exist as dynamic properties,
    /// and then passing them to the class constructor if needed.
    /// </summary>
    /// <param name="includeInstanceProperties">Whether to include instance properties (= 'static' properties on the class) as dynamic properties on the new instance. Default is true</param>
    member this.DeepCopyProperties(?includeInstanceProperties:bool) = 
        let includeInstanceProperties = defaultArg includeInstanceProperties true
        CopyUtils.tryDeepCopyObj(this, includeInstanceProperties)

    #if !FABLE_COMPILER
    // Some necessary overrides for methods inherited from System.Dynamic.DynamicObject()
    // 
    // Needed mainly for making Newtonsoft.Json Serialization work
    override this.TryGetMember(binder:GetMemberBinder,result:obj byref) =     
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

    member this.ReferenceEquals (other: DynamicObj) = System.Object.ReferenceEquals(this,other)

    member this.StructurallyEquals (other: DynamicObj) =
        this.GetHashCode() = other.GetHashCode()

    override this.GetHashCode () =
        HashUtils.deepHash this

    override this.Equals o =
        match o with
        | :? DynamicObj as other ->
            this.StructurallyEquals(other)
        | _ -> false

and HashUtils = 

    static member deepHash (o:obj) =
        match o with
        | :? DynamicObj as o ->
            o.GetProperties(true)
            |> Seq.sortBy (fun pair -> pair.Key)
            |> HashCodes.boxHashKeyValSeqBy HashUtils.deepHash
            |> fun x -> x :?> int
        | :? string as s -> DynamicObj.HashCodes.hash s
        #if !FABLE_COMPILER
        | :? System.Collections.IDictionary as d -> 
            let mutable en = d.GetEnumerator()
            [
                while en.MoveNext() do 
                    let c = en.Current :?> System.Collections.DictionaryEntry
                    HashCodes.mergeHashes (hash c.Key) (HashUtils.deepHash c.Value)
            ]
            |> List.reduce HashCodes.mergeHashes
        #endif
        | :? System.Collections.IEnumerable as e ->
            let en = e.GetEnumerator()
            [
                while en.MoveNext() do 
                    HashUtils.deepHash en.Current
            ]
            |> List.reduce HashCodes.mergeHashes
        | _ -> DynamicObj.HashCodes.hash o

and CopyUtils =

    /// <summary>
    /// function to deep copy a boxed object (if possible)

    /// The following cases are handled (in this precedence):
    ///
    /// - Basic F# types (`bool`, `byte`, `sbyte`, `int16`, `uint16`, `int`, `uint`, `int64`, `uint64`, `nativeint`, `unativeint`, `float`, `float32`, `char`, `string`, `unit`, `decimal`)
    /// 
    /// - `ResizeArrays` and `Dictionaries` containing any combination of basic F# types
    /// 
    /// - `Dictionaries` containing `DynamicObj` as keys or values in any combination with `DynamicObj` or basic F# types as keys or values
    /// 
    /// - `array&lt;DynamicObj&gt;`, `list&lt;DynamicObj&gt;`, `ResizeArray&lt;DynamicObj&gt;`: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
    /// 
    /// - `System.ICloneable`: If the property implements `ICloneable`, the `Clone()` method is called on the property.
    /// 
    /// - `DynamicObj` (and derived classes): properties that are themselves `DynamicObj` instances are deep copied recursively.
    ///   if a derived class has static properties (e.g. instance properties), these can be copied as dynamic properties on the new instance or ignored.
    /// 
    /// Note on Classes that inherit from `DynamicObj`:
    /// 
    /// Classes that inherit from DynamicObj will match the `DynamicObj` typecheck if they do not implement `ICloneable`.
    /// The deep copied instances will be cast to `DynamicObj` with deep copied dynamic properties. Staic/instance properties can be copied as dynamic properties on the new instance or be ignored.
    /// It should be possible to 'recover' the original type by checking if the needed properties exist as dynamic properties,
    /// and then passing them to the class constructor if needed.
    /// </summary>
    /// <param name="o">The object that should be deep copied</param>
    /// <param name="includeInstanceProperties">Whether to include instance properties (= 'static' properties on the class) as dynamic properties on the new instance for matched DynamicObj. Default is true</param>
    static member tryDeepCopyObj(
        o:obj,
        ?includeInstanceProperties:bool
    ) =
        let includeInstanceProperties = defaultArg includeInstanceProperties true

        let rec tryDeepCopyObj (o:obj) =
            match o with

            // might be that we do not need this case, however if we remove it, some types will match the 
            // ICloneable case in transpiled code, which we'd like to prevent, so well keep it for now.
            // https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/basic-types
            | :? bool
            | :? byte
            | :? sbyte 
            | :? int16
            | :? uint16
            | :? int
            | :? uint
            | :? int64
            | :? uint64
            #if !FABLE_COMPILER
            | :? nativeint
            | :? unativeint
            #endif
            | :? float
            | :? float32
            | :? char
            | :? string
            | :? unit
                -> o

            #if !FABLE_COMPILER_PYTHON
            // https://github.com/fable-compiler/Fable/issues/3971
            | :? decimal -> o
            #endif

            #if !FABLE_COMPILER

            // we can do some more type checking in F# land

            // ResizeArray typematches are all translated as `isArrayLike` in Fable JS/Py, so all these cases are the same in transpiled code.
            // However this is fine, as we can transpile one single case (the `ResizeArray<DynamicObj>` case) that recursively applies `tryDeepCopyObj` on all items.
            // That way, everything is fine in the untyped world, and we can keep the boxed types in F#.

            // ResizeArrays are mutable and we need to copy them. For primitives, we can do this easily.
            | :? ResizeArray<bool>       as r -> ResizeArray(r) |> box
            | :? ResizeArray<byte>       as r -> ResizeArray(r) |> box
            | :? ResizeArray<sbyte>      as r -> ResizeArray(r) |> box
            | :? ResizeArray<int16>      as r -> ResizeArray(r) |> box
            | :? ResizeArray<uint16>     as r -> ResizeArray(r) |> box
            | :? ResizeArray<int>        as r -> ResizeArray(r) |> box
            | :? ResizeArray<uint>       as r -> ResizeArray(r) |> box
            | :? ResizeArray<int64>      as r -> ResizeArray(r) |> box
            | :? ResizeArray<uint64>     as r -> ResizeArray(r) |> box
            | :? ResizeArray<nativeint>  as r -> ResizeArray(r) |> box
            | :? ResizeArray<unativeint> as r -> ResizeArray(r) |> box
            | :? ResizeArray<float>      as r -> ResizeArray(r) |> box
            | :? ResizeArray<float32>    as r -> ResizeArray(r) |> box
            | :? ResizeArray<char>       as r -> ResizeArray(r) |> box
            | :? ResizeArray<string>     as r -> ResizeArray(r) |> box
            | :? ResizeArray<unit>       as r -> ResizeArray(r) |> box
            | :? ResizeArray<decimal>    as r -> ResizeArray(r) |> box

            // Dictionaries are mutable and we need to copy them. For primitives, we can do this easily, it is just a lot of work to write man
            // Fable does simply not compile these typechecks, i guess because everything is an object/dictionary in JS/PY.
            | :? Dictionary<bool,bool>         as dict -> Dictionary<bool,bool>(dict) |> box
            | :? Dictionary<bool,byte>         as dict -> Dictionary<bool,byte>(dict) |> box
            | :? Dictionary<bool,sbyte>        as dict -> Dictionary<bool,sbyte>(dict) |> box
            | :? Dictionary<bool,int16>        as dict -> Dictionary<bool,int16>(dict) |> box
            | :? Dictionary<bool,uint16>       as dict -> Dictionary<bool,uint16>(dict) |> box
            | :? Dictionary<bool,int>          as dict -> Dictionary<bool,int>(dict) |> box
            | :? Dictionary<bool,uint>         as dict -> Dictionary<bool,uint>(dict) |> box
            | :? Dictionary<bool,int64>        as dict -> Dictionary<bool,int64>(dict) |> box
            | :? Dictionary<bool,uint64>       as dict -> Dictionary<bool,uint64>(dict) |> box
            | :? Dictionary<bool,nativeint>    as dict -> Dictionary<bool,nativeint>(dict) |> box
            | :? Dictionary<bool,unativeint>   as dict -> Dictionary<bool,unativeint>(dict) |> box
            | :? Dictionary<bool,float>        as dict -> Dictionary<bool,float>(dict) |> box
            | :? Dictionary<bool,float32>      as dict -> Dictionary<bool,float32>(dict) |> box
            | :? Dictionary<bool,char>         as dict -> Dictionary<bool,char>(dict) |> box
            | :? Dictionary<bool,string>       as dict -> Dictionary<bool,string>(dict) |> box
            | :? Dictionary<bool,unit>         as dict -> Dictionary<bool,unit>(dict) |> box
            | :? Dictionary<bool,decimal>      as dict -> Dictionary<bool,decimal>(dict) |> box
            
            | :? Dictionary<byte,bool>         as dict -> Dictionary<byte,bool>(dict) |> box
            | :? Dictionary<byte,byte>         as dict -> Dictionary<byte,byte>(dict) |> box
            | :? Dictionary<byte,sbyte>        as dict -> Dictionary<byte,sbyte>(dict) |> box
            | :? Dictionary<byte,int16>        as dict -> Dictionary<byte,int16>(dict) |> box
            | :? Dictionary<byte,uint16>       as dict -> Dictionary<byte,uint16>(dict) |> box
            | :? Dictionary<byte,int>          as dict -> Dictionary<byte,int>(dict) |> box
            | :? Dictionary<byte,uint>         as dict -> Dictionary<byte,uint>(dict) |> box
            | :? Dictionary<byte,int64>        as dict -> Dictionary<byte,int64>(dict) |> box
            | :? Dictionary<byte,uint64>       as dict -> Dictionary<byte,uint64>(dict) |> box
            | :? Dictionary<byte,nativeint>    as dict -> Dictionary<byte,nativeint>(dict) |> box
            | :? Dictionary<byte,unativeint>   as dict -> Dictionary<byte,unativeint>(dict) |> box
            | :? Dictionary<byte,float>        as dict -> Dictionary<byte,float>(dict) |> box
            | :? Dictionary<byte,float32>      as dict -> Dictionary<byte,float32>(dict) |> box
            | :? Dictionary<byte,char>         as dict -> Dictionary<byte,char>(dict) |> box
            | :? Dictionary<byte,string>       as dict -> Dictionary<byte,string>(dict) |> box
            | :? Dictionary<byte,unit>         as dict -> Dictionary<byte,unit>(dict) |> box
            | :? Dictionary<byte,decimal>      as dict -> Dictionary<byte,decimal>(dict) |> box
            
            | :? Dictionary<sbyte,bool>         as dict -> Dictionary<sbyte,bool>(dict) |> box
            | :? Dictionary<sbyte,byte>         as dict -> Dictionary<sbyte,byte>(dict) |> box
            | :? Dictionary<sbyte,sbyte>        as dict -> Dictionary<sbyte,sbyte>(dict) |> box
            | :? Dictionary<sbyte,int16>        as dict -> Dictionary<sbyte,int16>(dict) |> box
            | :? Dictionary<sbyte,uint16>       as dict -> Dictionary<sbyte,uint16>(dict) |> box
            | :? Dictionary<sbyte,int>          as dict -> Dictionary<sbyte,int>(dict) |> box
            | :? Dictionary<sbyte,uint>         as dict -> Dictionary<sbyte,uint>(dict) |> box
            | :? Dictionary<sbyte,int64>        as dict -> Dictionary<sbyte,int64>(dict) |> box
            | :? Dictionary<sbyte,uint64>       as dict -> Dictionary<sbyte,uint64>(dict) |> box
            | :? Dictionary<sbyte,nativeint>    as dict -> Dictionary<sbyte,nativeint>(dict) |> box
            | :? Dictionary<sbyte,unativeint>   as dict -> Dictionary<sbyte,unativeint>(dict) |> box
            | :? Dictionary<sbyte,float>        as dict -> Dictionary<sbyte,float>(dict) |> box
            | :? Dictionary<sbyte,float32>      as dict -> Dictionary<sbyte,float32>(dict) |> box
            | :? Dictionary<sbyte,char>         as dict -> Dictionary<sbyte,char>(dict) |> box
            | :? Dictionary<sbyte,string>       as dict -> Dictionary<sbyte,string>(dict) |> box
            | :? Dictionary<sbyte,unit>         as dict -> Dictionary<sbyte,unit>(dict) |> box
            | :? Dictionary<sbyte,decimal>      as dict -> Dictionary<sbyte,decimal>(dict) |> box
            
            | :? Dictionary<int16,bool>         as dict -> Dictionary<int16,bool>(dict) |> box
            | :? Dictionary<int16,byte>         as dict -> Dictionary<int16,byte>(dict) |> box
            | :? Dictionary<int16,sbyte>        as dict -> Dictionary<int16,sbyte>(dict) |> box
            | :? Dictionary<int16,int16>        as dict -> Dictionary<int16,int16>(dict) |> box
            | :? Dictionary<int16,uint16>       as dict -> Dictionary<int16,uint16>(dict) |> box
            | :? Dictionary<int16,int>          as dict -> Dictionary<int16,int>(dict) |> box
            | :? Dictionary<int16,uint>         as dict -> Dictionary<int16,uint>(dict) |> box
            | :? Dictionary<int16,int64>        as dict -> Dictionary<int16,int64>(dict) |> box
            | :? Dictionary<int16,uint64>       as dict -> Dictionary<int16,uint64>(dict) |> box
            | :? Dictionary<int16,nativeint>    as dict -> Dictionary<int16,nativeint>(dict) |> box
            | :? Dictionary<int16,unativeint>   as dict -> Dictionary<int16,unativeint>(dict) |> box
            | :? Dictionary<int16,float>        as dict -> Dictionary<int16,float>(dict) |> box
            | :? Dictionary<int16,float32>      as dict -> Dictionary<int16,float32>(dict) |> box
            | :? Dictionary<int16,char>         as dict -> Dictionary<int16,char>(dict) |> box
            | :? Dictionary<int16,string>       as dict -> Dictionary<int16,string>(dict) |> box
            | :? Dictionary<int16,unit>         as dict -> Dictionary<int16,unit>(dict) |> box
            | :? Dictionary<int16,decimal>      as dict -> Dictionary<int16,decimal>(dict) |> box
            
            | :? Dictionary<uint16,bool>         as dict -> Dictionary<uint16,bool>(dict) |> box
            | :? Dictionary<uint16,byte>         as dict -> Dictionary<uint16,byte>(dict) |> box
            | :? Dictionary<uint16,sbyte>        as dict -> Dictionary<uint16,sbyte>(dict) |> box
            | :? Dictionary<uint16,int16>        as dict -> Dictionary<uint16,int16>(dict) |> box
            | :? Dictionary<uint16,uint16>       as dict -> Dictionary<uint16,uint16>(dict) |> box
            | :? Dictionary<uint16,int>          as dict -> Dictionary<uint16,int>(dict) |> box
            | :? Dictionary<uint16,uint>         as dict -> Dictionary<uint16,uint>(dict) |> box
            | :? Dictionary<uint16,int64>        as dict -> Dictionary<uint16,int64>(dict) |> box
            | :? Dictionary<uint16,uint64>       as dict -> Dictionary<uint16,uint64>(dict) |> box
            | :? Dictionary<uint16,nativeint>    as dict -> Dictionary<uint16,nativeint>(dict) |> box
            | :? Dictionary<uint16,unativeint>   as dict -> Dictionary<uint16,unativeint>(dict) |> box
            | :? Dictionary<uint16,float>        as dict -> Dictionary<uint16,float>(dict) |> box
            | :? Dictionary<uint16,float32>      as dict -> Dictionary<uint16,float32>(dict) |> box
            | :? Dictionary<uint16,char>         as dict -> Dictionary<uint16,char>(dict) |> box
            | :? Dictionary<uint16,string>       as dict -> Dictionary<uint16,string>(dict) |> box
            | :? Dictionary<uint16,unit>         as dict -> Dictionary<uint16,unit>(dict) |> box
            | :? Dictionary<uint16,decimal>      as dict -> Dictionary<uint16,decimal>(dict) |> box
            
            | :? Dictionary<int,bool>         as dict -> Dictionary<int,bool>(dict) |> box
            | :? Dictionary<int,byte>         as dict -> Dictionary<int,byte>(dict) |> box
            | :? Dictionary<int,sbyte>        as dict -> Dictionary<int,sbyte>(dict) |> box
            | :? Dictionary<int,int16>        as dict -> Dictionary<int,int16>(dict) |> box
            | :? Dictionary<int,uint16>       as dict -> Dictionary<int,uint16>(dict) |> box
            | :? Dictionary<int,int>          as dict -> Dictionary<int,int>(dict) |> box
            | :? Dictionary<int,uint>         as dict -> Dictionary<int,uint>(dict) |> box
            | :? Dictionary<int,int64>        as dict -> Dictionary<int,int64>(dict) |> box
            | :? Dictionary<int,uint64>       as dict -> Dictionary<int,uint64>(dict) |> box
            | :? Dictionary<int,nativeint>    as dict -> Dictionary<int,nativeint>(dict) |> box
            | :? Dictionary<int,unativeint>   as dict -> Dictionary<int,unativeint>(dict) |> box
            | :? Dictionary<int,float>        as dict -> Dictionary<int,float>(dict) |> box
            | :? Dictionary<int,float32>      as dict -> Dictionary<int,float32>(dict) |> box
            | :? Dictionary<int,char>         as dict -> Dictionary<int,char>(dict) |> box
            | :? Dictionary<int,string>       as dict -> Dictionary<int,string>(dict) |> box
            | :? Dictionary<int,unit>         as dict -> Dictionary<int,unit>(dict) |> box
            | :? Dictionary<int,decimal>      as dict -> Dictionary<int,decimal>(dict) |> box
            
            | :? Dictionary<uint,bool>         as dict -> Dictionary<uint,bool>(dict) |> box
            | :? Dictionary<uint,byte>         as dict -> Dictionary<uint,byte>(dict) |> box
            | :? Dictionary<uint,sbyte>        as dict -> Dictionary<uint,sbyte>(dict) |> box
            | :? Dictionary<uint,int16>        as dict -> Dictionary<uint,int16>(dict) |> box
            | :? Dictionary<uint,uint16>       as dict -> Dictionary<uint,uint16>(dict) |> box
            | :? Dictionary<uint,int>          as dict -> Dictionary<uint,int>(dict) |> box
            | :? Dictionary<uint,uint>         as dict -> Dictionary<uint,uint>(dict) |> box
            | :? Dictionary<uint,int64>        as dict -> Dictionary<uint,int64>(dict) |> box
            | :? Dictionary<uint,uint64>       as dict -> Dictionary<uint,uint64>(dict) |> box
            | :? Dictionary<uint,nativeint>    as dict -> Dictionary<uint,nativeint>(dict) |> box
            | :? Dictionary<uint,unativeint>   as dict -> Dictionary<uint,unativeint>(dict) |> box
            | :? Dictionary<uint,float>        as dict -> Dictionary<uint,float>(dict) |> box
            | :? Dictionary<uint,float32>      as dict -> Dictionary<uint,float32>(dict) |> box
            | :? Dictionary<uint,char>         as dict -> Dictionary<uint,char>(dict) |> box
            | :? Dictionary<uint,string>       as dict -> Dictionary<uint,string>(dict) |> box
            | :? Dictionary<uint,unit>         as dict -> Dictionary<uint,unit>(dict) |> box
            | :? Dictionary<uint,decimal>      as dict -> Dictionary<uint,decimal>(dict) |> box
            
            | :? Dictionary<int64,bool>         as dict -> Dictionary<int64,bool>(dict) |> box
            | :? Dictionary<int64,byte>         as dict -> Dictionary<int64,byte>(dict) |> box
            | :? Dictionary<int64,sbyte>        as dict -> Dictionary<int64,sbyte>(dict) |> box
            | :? Dictionary<int64,int16>        as dict -> Dictionary<int64,int16>(dict) |> box
            | :? Dictionary<int64,uint16>       as dict -> Dictionary<int64,uint16>(dict) |> box
            | :? Dictionary<int64,int>          as dict -> Dictionary<int64,int>(dict) |> box
            | :? Dictionary<int64,uint>         as dict -> Dictionary<int64,uint>(dict) |> box
            | :? Dictionary<int64,int64>        as dict -> Dictionary<int64,int64>(dict) |> box
            | :? Dictionary<int64,uint64>       as dict -> Dictionary<int64,uint64>(dict) |> box
            | :? Dictionary<int64,nativeint>    as dict -> Dictionary<int64,nativeint>(dict) |> box
            | :? Dictionary<int64,unativeint>   as dict -> Dictionary<int64,unativeint>(dict) |> box
            | :? Dictionary<int64,float>        as dict -> Dictionary<int64,float>(dict) |> box
            | :? Dictionary<int64,float32>      as dict -> Dictionary<int64,float32>(dict) |> box
            | :? Dictionary<int64,char>         as dict -> Dictionary<int64,char>(dict) |> box
            | :? Dictionary<int64,string>       as dict -> Dictionary<int64,string>(dict) |> box
            | :? Dictionary<int64,unit>         as dict -> Dictionary<int64,unit>(dict) |> box
            | :? Dictionary<int64,decimal>      as dict -> Dictionary<int64,decimal>(dict) |> box
            
            | :? Dictionary<uint64,bool>         as dict -> Dictionary<uint64,bool>(dict) |> box
            | :? Dictionary<uint64,byte>         as dict -> Dictionary<uint64,byte>(dict) |> box
            | :? Dictionary<uint64,sbyte>        as dict -> Dictionary<uint64,sbyte>(dict) |> box
            | :? Dictionary<uint64,int16>        as dict -> Dictionary<uint64,int16>(dict) |> box
            | :? Dictionary<uint64,uint16>       as dict -> Dictionary<uint64,uint16>(dict) |> box
            | :? Dictionary<uint64,int>          as dict -> Dictionary<uint64,int>(dict) |> box
            | :? Dictionary<uint64,uint>         as dict -> Dictionary<uint64,uint>(dict) |> box
            | :? Dictionary<uint64,int64>        as dict -> Dictionary<uint64,int64>(dict) |> box
            | :? Dictionary<uint64,uint64>       as dict -> Dictionary<uint64,uint64>(dict) |> box
            | :? Dictionary<uint64,nativeint>    as dict -> Dictionary<uint64,nativeint>(dict) |> box
            | :? Dictionary<uint64,unativeint>   as dict -> Dictionary<uint64,unativeint>(dict) |> box
            | :? Dictionary<uint64,float>        as dict -> Dictionary<uint64,float>(dict) |> box
            | :? Dictionary<uint64,float32>      as dict -> Dictionary<uint64,float32>(dict) |> box
            | :? Dictionary<uint64,char>         as dict -> Dictionary<uint64,char>(dict) |> box
            | :? Dictionary<uint64,string>       as dict -> Dictionary<uint64,string>(dict) |> box
            | :? Dictionary<uint64,unit>         as dict -> Dictionary<uint64,unit>(dict) |> box
            | :? Dictionary<uint64,decimal>      as dict -> Dictionary<uint64,decimal>(dict) |> box
            
            | :? Dictionary<nativeint,bool>         as dict -> Dictionary<nativeint,bool>(dict) |> box
            | :? Dictionary<nativeint,byte>         as dict -> Dictionary<nativeint,byte>(dict) |> box
            | :? Dictionary<nativeint,sbyte>        as dict -> Dictionary<nativeint,sbyte>(dict) |> box
            | :? Dictionary<nativeint,int16>        as dict -> Dictionary<nativeint,int16>(dict) |> box
            | :? Dictionary<nativeint,uint16>       as dict -> Dictionary<nativeint,uint16>(dict) |> box
            | :? Dictionary<nativeint,int>          as dict -> Dictionary<nativeint,int>(dict) |> box
            | :? Dictionary<nativeint,uint>         as dict -> Dictionary<nativeint,uint>(dict) |> box
            | :? Dictionary<nativeint,int64>        as dict -> Dictionary<nativeint,int64>(dict) |> box
            | :? Dictionary<nativeint,uint64>       as dict -> Dictionary<nativeint,uint64>(dict) |> box
            | :? Dictionary<nativeint,nativeint>    as dict -> Dictionary<nativeint,nativeint>(dict) |> box
            | :? Dictionary<nativeint,unativeint>   as dict -> Dictionary<nativeint,unativeint>(dict) |> box
            | :? Dictionary<nativeint,float>        as dict -> Dictionary<nativeint,float>(dict) |> box
            | :? Dictionary<nativeint,float32>      as dict -> Dictionary<nativeint,float32>(dict) |> box
            | :? Dictionary<nativeint,char>         as dict -> Dictionary<nativeint,char>(dict) |> box
            | :? Dictionary<nativeint,string>       as dict -> Dictionary<nativeint,string>(dict) |> box
            | :? Dictionary<nativeint,unit>         as dict -> Dictionary<nativeint,unit>(dict) |> box
            | :? Dictionary<nativeint,decimal>      as dict -> Dictionary<nativeint,decimal>(dict) |> box
            
            | :? Dictionary<unativeint,bool>         as dict -> Dictionary<unativeint,bool>(dict) |> box
            | :? Dictionary<unativeint,byte>         as dict -> Dictionary<unativeint,byte>(dict) |> box
            | :? Dictionary<unativeint,sbyte>        as dict -> Dictionary<unativeint,sbyte>(dict) |> box
            | :? Dictionary<unativeint,int16>        as dict -> Dictionary<unativeint,int16>(dict) |> box
            | :? Dictionary<unativeint,uint16>       as dict -> Dictionary<unativeint,uint16>(dict) |> box
            | :? Dictionary<unativeint,int>          as dict -> Dictionary<unativeint,int>(dict) |> box
            | :? Dictionary<unativeint,uint>         as dict -> Dictionary<unativeint,uint>(dict) |> box
            | :? Dictionary<unativeint,int64>        as dict -> Dictionary<unativeint,int64>(dict) |> box
            | :? Dictionary<unativeint,uint64>       as dict -> Dictionary<unativeint,uint64>(dict) |> box
            | :? Dictionary<unativeint,nativeint>    as dict -> Dictionary<unativeint,nativeint>(dict) |> box
            | :? Dictionary<unativeint,unativeint>   as dict -> Dictionary<unativeint,unativeint>(dict) |> box
            | :? Dictionary<unativeint,float>        as dict -> Dictionary<unativeint,float>(dict) |> box
            | :? Dictionary<unativeint,float32>      as dict -> Dictionary<unativeint,float32>(dict) |> box
            | :? Dictionary<unativeint,char>         as dict -> Dictionary<unativeint,char>(dict) |> box
            | :? Dictionary<unativeint,string>       as dict -> Dictionary<unativeint,string>(dict) |> box
            | :? Dictionary<unativeint,unit>         as dict -> Dictionary<unativeint,unit>(dict) |> box
            | :? Dictionary<unativeint,decimal>      as dict -> Dictionary<unativeint,decimal>(dict) |> box

            | :? Dictionary<float,bool>         as dict -> Dictionary<float,bool>(dict) |> box
            | :? Dictionary<float,byte>         as dict -> Dictionary<float,byte>(dict) |> box
            | :? Dictionary<float,sbyte>        as dict -> Dictionary<float,sbyte>(dict) |> box
            | :? Dictionary<float,int16>        as dict -> Dictionary<float,int16>(dict) |> box
            | :? Dictionary<float,uint16>       as dict -> Dictionary<float,uint16>(dict) |> box
            | :? Dictionary<float,int>          as dict -> Dictionary<float,int>(dict) |> box
            | :? Dictionary<float,uint>         as dict -> Dictionary<float,uint>(dict) |> box
            | :? Dictionary<float,int64>        as dict -> Dictionary<float,int64>(dict) |> box
            | :? Dictionary<float,uint64>       as dict -> Dictionary<float,uint64>(dict) |> box
            | :? Dictionary<float,nativeint>    as dict -> Dictionary<float,nativeint>(dict) |> box
            | :? Dictionary<float,unativeint>   as dict -> Dictionary<float,unativeint>(dict) |> box
            | :? Dictionary<float,float>        as dict -> Dictionary<float,float>(dict) |> box
            | :? Dictionary<float,float32>      as dict -> Dictionary<float,float32>(dict) |> box
            | :? Dictionary<float,char>         as dict -> Dictionary<float,char>(dict) |> box
            | :? Dictionary<float,string>       as dict -> Dictionary<float,string>(dict) |> box
            | :? Dictionary<float,unit>         as dict -> Dictionary<float,unit>(dict) |> box
            | :? Dictionary<float,decimal>      as dict -> Dictionary<float,decimal>(dict) |> box
            
            | :? Dictionary<float32,bool>         as dict -> Dictionary<float32,bool>(dict) |> box
            | :? Dictionary<float32,byte>         as dict -> Dictionary<float32,byte>(dict) |> box
            | :? Dictionary<float32,sbyte>        as dict -> Dictionary<float32,sbyte>(dict) |> box
            | :? Dictionary<float32,int16>        as dict -> Dictionary<float32,int16>(dict) |> box
            | :? Dictionary<float32,uint16>       as dict -> Dictionary<float32,uint16>(dict) |> box
            | :? Dictionary<float32,int>          as dict -> Dictionary<float32,int>(dict) |> box
            | :? Dictionary<float32,uint>         as dict -> Dictionary<float32,uint>(dict) |> box
            | :? Dictionary<float32,int64>        as dict -> Dictionary<float32,int64>(dict) |> box
            | :? Dictionary<float32,uint64>       as dict -> Dictionary<float32,uint64>(dict) |> box
            | :? Dictionary<float32,nativeint>    as dict -> Dictionary<float32,nativeint>(dict) |> box
            | :? Dictionary<float32,unativeint>   as dict -> Dictionary<float32,unativeint>(dict) |> box
            | :? Dictionary<float32,float>        as dict -> Dictionary<float32,float>(dict) |> box
            | :? Dictionary<float32,float32>      as dict -> Dictionary<float32,float32>(dict) |> box
            | :? Dictionary<float32,char>         as dict -> Dictionary<float32,char>(dict) |> box
            | :? Dictionary<float32,string>       as dict -> Dictionary<float32,string>(dict) |> box
            | :? Dictionary<float32,unit>         as dict -> Dictionary<float32,unit>(dict) |> box
            | :? Dictionary<float32,decimal>      as dict -> Dictionary<float32,decimal>(dict) |> box
            
            | :? Dictionary<char,bool>         as dict -> Dictionary<char,bool>(dict) |> box
            | :? Dictionary<char,byte>         as dict -> Dictionary<char,byte>(dict) |> box
            | :? Dictionary<char,sbyte>        as dict -> Dictionary<char,sbyte>(dict) |> box
            | :? Dictionary<char,int16>        as dict -> Dictionary<char,int16>(dict) |> box
            | :? Dictionary<char,uint16>       as dict -> Dictionary<char,uint16>(dict) |> box
            | :? Dictionary<char,int>          as dict -> Dictionary<char,int>(dict) |> box
            | :? Dictionary<char,uint>         as dict -> Dictionary<char,uint>(dict) |> box
            | :? Dictionary<char,int64>        as dict -> Dictionary<char,int64>(dict) |> box
            | :? Dictionary<char,uint64>       as dict -> Dictionary<char,uint64>(dict) |> box
            | :? Dictionary<char,nativeint>    as dict -> Dictionary<char,nativeint>(dict) |> box
            | :? Dictionary<char,unativeint>   as dict -> Dictionary<char,unativeint>(dict) |> box
            | :? Dictionary<char,float>        as dict -> Dictionary<char,float>(dict) |> box
            | :? Dictionary<char,float32>      as dict -> Dictionary<char,float32>(dict) |> box
            | :? Dictionary<char,char>         as dict -> Dictionary<char,char>(dict) |> box
            | :? Dictionary<char,string>       as dict -> Dictionary<char,string>(dict) |> box
            | :? Dictionary<char,unit>         as dict -> Dictionary<char,unit>(dict) |> box
            | :? Dictionary<char,decimal>      as dict -> Dictionary<char,decimal>(dict) |> box
            
            | :? Dictionary<string,bool>         as dict -> Dictionary<string,bool>(dict) |> box
            | :? Dictionary<string,byte>         as dict -> Dictionary<string,byte>(dict) |> box
            | :? Dictionary<string,sbyte>        as dict -> Dictionary<string,sbyte>(dict) |> box
            | :? Dictionary<string,int16>        as dict -> Dictionary<string,int16>(dict) |> box
            | :? Dictionary<string,uint16>       as dict -> Dictionary<string,uint16>(dict) |> box
            | :? Dictionary<string,int>          as dict -> Dictionary<string,int>(dict) |> box
            | :? Dictionary<string,uint>         as dict -> Dictionary<string,uint>(dict) |> box
            | :? Dictionary<string,int64>        as dict -> Dictionary<string,int64>(dict) |> box
            | :? Dictionary<string,uint64>       as dict -> Dictionary<string,uint64>(dict) |> box
            | :? Dictionary<string,nativeint>    as dict -> Dictionary<string,nativeint>(dict) |> box
            | :? Dictionary<string,unativeint>   as dict -> Dictionary<string,unativeint>(dict) |> box
            | :? Dictionary<string,float>        as dict -> Dictionary<string,float>(dict) |> box
            | :? Dictionary<string,float32>      as dict -> Dictionary<string,float32>(dict) |> box
            | :? Dictionary<string,char>         as dict -> Dictionary<string,char>(dict) |> box
            | :? Dictionary<string,string>       as dict -> Dictionary<string,string>(dict) |> box
            | :? Dictionary<string,unit>         as dict -> Dictionary<string,unit>(dict) |> box
            | :? Dictionary<string,decimal>      as dict -> Dictionary<string,decimal>(dict) |> box
            
            | :? Dictionary<unit,bool>         as dict -> Dictionary<unit,bool>(dict) |> box
            | :? Dictionary<unit,byte>         as dict -> Dictionary<unit,byte>(dict) |> box
            | :? Dictionary<unit,sbyte>        as dict -> Dictionary<unit,sbyte>(dict) |> box
            | :? Dictionary<unit,int16>        as dict -> Dictionary<unit,int16>(dict) |> box
            | :? Dictionary<unit,uint16>       as dict -> Dictionary<unit,uint16>(dict) |> box
            | :? Dictionary<unit,int>          as dict -> Dictionary<unit,int>(dict) |> box
            | :? Dictionary<unit,uint>         as dict -> Dictionary<unit,uint>(dict) |> box
            | :? Dictionary<unit,int64>        as dict -> Dictionary<unit,int64>(dict) |> box
            | :? Dictionary<unit,uint64>       as dict -> Dictionary<unit,uint64>(dict) |> box
            | :? Dictionary<unit,nativeint>    as dict -> Dictionary<unit,nativeint>(dict) |> box
            | :? Dictionary<unit,unativeint>   as dict -> Dictionary<unit,unativeint>(dict) |> box
            | :? Dictionary<unit,float>        as dict -> Dictionary<unit,float>(dict) |> box
            | :? Dictionary<unit,float32>      as dict -> Dictionary<unit,float32>(dict) |> box
            | :? Dictionary<unit,char>         as dict -> Dictionary<unit,char>(dict) |> box
            | :? Dictionary<unit,string>       as dict -> Dictionary<unit,string>(dict) |> box
            | :? Dictionary<unit,unit>         as dict -> Dictionary<unit,unit>(dict) |> box
            | :? Dictionary<unit,decimal>      as dict -> Dictionary<unit,decimal>(dict) |> box

            | :? Dictionary<decimal,bool>         as dict -> Dictionary<decimal,bool>(dict) |> box
            | :? Dictionary<decimal,byte>         as dict -> Dictionary<decimal,byte>(dict) |> box
            | :? Dictionary<decimal,sbyte>        as dict -> Dictionary<decimal,sbyte>(dict) |> box
            | :? Dictionary<decimal,int16>        as dict -> Dictionary<decimal,int16>(dict) |> box
            | :? Dictionary<decimal,uint16>       as dict -> Dictionary<decimal,uint16>(dict) |> box
            | :? Dictionary<decimal,int>          as dict -> Dictionary<decimal,int>(dict) |> box
            | :? Dictionary<decimal,uint>         as dict -> Dictionary<decimal,uint>(dict) |> box
            | :? Dictionary<decimal,int64>        as dict -> Dictionary<decimal,int64>(dict) |> box
            | :? Dictionary<decimal,uint64>       as dict -> Dictionary<decimal,uint64>(dict) |> box
            | :? Dictionary<decimal,nativeint>    as dict -> Dictionary<decimal,nativeint>(dict) |> box
            | :? Dictionary<decimal,unativeint>   as dict -> Dictionary<decimal,unativeint>(dict) |> box
            | :? Dictionary<decimal,float>        as dict -> Dictionary<decimal,float>(dict) |> box
            | :? Dictionary<decimal,float32>      as dict -> Dictionary<decimal,float32>(dict) |> box
            | :? Dictionary<decimal,char>         as dict -> Dictionary<decimal,char>(dict) |> box
            | :? Dictionary<decimal,string>       as dict -> Dictionary<decimal,string>(dict) |> box
            | :? Dictionary<decimal,unit>         as dict -> Dictionary<decimal,unit>(dict) |> box
            | :? Dictionary<decimal,decimal>      as dict -> Dictionary<decimal,decimal>(dict) |> box

            // same for dictionaries containing DynamicObj as value
            | :? Dictionary<bool,DynamicObj>         as dict -> 
                let newDict = Dictionary<bool,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<byte,DynamicObj>         as dict -> 
                let newDict = Dictionary<byte,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<sbyte ,DynamicObj>       as dict -> 
                let newDict = Dictionary<sbyte ,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<int16,DynamicObj>        as dict -> 
                let newDict = Dictionary<int16,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<uint16,DynamicObj>       as dict -> 
                let newDict = Dictionary<uint16,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<int,DynamicObj>          as dict -> 
                let newDict = Dictionary<int,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<uint,DynamicObj>         as dict -> 
                let newDict = Dictionary<uint,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<int64,DynamicObj>        as dict -> 
                let newDict = Dictionary<int64,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<uint64,DynamicObj>       as dict -> 
                let newDict = Dictionary<uint64,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<nativeint,DynamicObj>    as dict -> 
                let newDict = Dictionary<nativeint,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<unativeint,DynamicObj>   as dict -> 
                let newDict = Dictionary<unativeint,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<float,DynamicObj>        as dict -> 
                let newDict = Dictionary<float,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<float32,DynamicObj>      as dict -> 
                let newDict = Dictionary<float32,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<char,DynamicObj>         as dict -> 
                let newDict = Dictionary<char,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<string,DynamicObj>       as dict -> 
                let newDict = Dictionary<string,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            | :? Dictionary<unit,DynamicObj>         as dict -> 
                let newDict = Dictionary<unit,DynamicObj>()
                for kv in dict do newDict.Add(kv.Key, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box

            // And for dictionaries containing DynamicObj as key
            | :? Dictionary<DynamicObj,bool>         as dict -> 
                let newDict = Dictionary<DynamicObj,bool>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,byte>         as dict -> 
                let newDict = Dictionary<DynamicObj,byte>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,sbyte>       as dict -> 
                let newDict = Dictionary<DynamicObj,sbyte>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,int16>        as dict -> 
                let newDict = Dictionary<DynamicObj,int16>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,uint16>       as dict -> 
                let newDict = Dictionary<DynamicObj,uint16>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,int>          as dict -> 
                let newDict = Dictionary<DynamicObj,int>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,uint>         as dict -> 
                let newDict = Dictionary<DynamicObj,uint>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,int64>        as dict -> 
                let newDict = Dictionary<DynamicObj,int64>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,uint64>       as dict -> 
                let newDict = Dictionary<DynamicObj,uint64>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,nativeint>    as dict -> 
                let newDict = Dictionary<DynamicObj,nativeint>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,unativeint>   as dict -> 
                let newDict = Dictionary<DynamicObj,unativeint>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,float>        as dict -> 
                let newDict = Dictionary<DynamicObj,float>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,float32>      as dict -> 
                let newDict = Dictionary<DynamicObj,float32>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,char>         as dict -> 
                let newDict = Dictionary<DynamicObj,char>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,string>       as dict -> 
                let newDict = Dictionary<DynamicObj,string>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box
            | :? Dictionary<DynamicObj,unit>         as dict -> 
                let newDict = Dictionary<DynamicObj,unit>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, kv.Value)
                newDict |> box            
            | :? Dictionary<DynamicObj,DynamicObj>         as dict -> 
                let newDict = Dictionary<DynamicObj,DynamicObj>()
                for kv in dict do newDict.Add(tryDeepCopyObj kv.Key :?> DynamicObj, tryDeepCopyObj kv.Value :?> DynamicObj)
                newDict |> box
            #endif

            // native fallbacks for matching Map/dict, see https://github.com/CSBiology/DynamicObj/issues/47

            #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
            | o when FableJS.Dictionaries.isMap o -> 
                let o = o |> unbox<Dictionary<obj,obj>>
                let newDict = Dictionary<obj,obj>()
                for kv in o do newDict.Add(tryDeepCopyObj kv.Key, tryDeepCopyObj kv.Value)
                newDict |> box
            | o when FableJS.Dictionaries.isDict o -> 
                let o = o |> unbox<Dictionary<obj,obj>>
                let newDict = Dictionary<obj,obj>()
                for kv in o do newDict.Add(tryDeepCopyObj kv.Key, tryDeepCopyObj kv.Value)
                newDict |> box
            #endif
            #if FABLE_COMPILER_PYTHON
            // https://github.com/fable-compiler/Fable/issues/3972
            | o when FablePy.Dictionaries.isDict o ->
                let o = o |> unbox<Dictionary<obj,obj>>
                let newDict = Dictionary<obj,obj>()
                for kv in o do newDict.Add(tryDeepCopyObj kv.Key, tryDeepCopyObj kv.Value)
                newDict |> box
            #endif

            // These collections of DynamicObj can be cloned recursively
            | :? ResizeArray<DynamicObj> as dyns ->
                box (ResizeArray([for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj]))
            #if !FABLE_COMPILER
            // this gets compiled to isArrayLike just the same as ResizeArray, but generates a slightly different result handling, so we only transpile the above.
            | :? array<DynamicObj> as dyns ->
                box [|for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj|]
            #endif
            | :? list<DynamicObj> as dyns ->
                box [for dyn in dyns -> tryDeepCopyObj dyn :?> DynamicObj]

            // Fable compilable version of typematching against implemented IClonable
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
                for kv in (dyn.GetProperties(includeInstanceProperties)) do
                    newDyn.SetProperty(kv.Key, tryDeepCopyObj kv.Value)
                box newDyn
            | _ -> o

        tryDeepCopyObj o