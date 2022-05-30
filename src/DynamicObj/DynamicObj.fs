namespace rec DynamicObj

open System.Dynamic
open System.Collections.Generic

open Newtonsoft.Json

type DynamicObj internal (dict:Dictionary<string, obj>) = 
    
    inherit DynamicObject () 
    
    let properties = dict//new Dictionary<string, obj>()

    member private this.Properties = properties

    /// 
    new () = DynamicObj(new Dictionary<string, obj>())

    /// Gets property value
    member this.TryGetValue name = 
        // first check the Properties collection for member
        match properties.TryGetValue name with
        | true,value ->  Some value
        // Next check for Public properties via Reflection
        | _ -> ReflectionUtils.tryGetPropertyValue this name


    /// Gets property value
    member this.TryGetTypedValue<'a> name = 
        match (this.TryGetValue name) with
        | None -> None
        | Some o -> 
            match o with
            | :? 'a -> o :?> 'a |> Some
            | _ -> None

        
    /// Sets property value, creating a new property if none exists
    member this.SetValue (name,value) = // private
        // first check to see if there's a native property to set

        match ReflectionUtils.tryGetPropertyInfo this name with
        | Some property ->
            try 
                // let t = property.ReflectedType
                // t.InvokeMember(name,Reflection.BindingFlags.SetProperty,null,this,[|value|]) |> ignore

                //let tmp = Convert.ChangeType(this, property.ReflectedType)
                //let tmp = downcast this : (typeof<t.GetType()>)
                property.SetValue(this, value, null)
            with
            | :? System.ArgumentException -> raise <| System.ArgumentException("Readonly property - Property set method not found.")
            | :? System.NullReferenceException -> raise <| System.NullReferenceException()
        
        | None -> 
            // Next check the Properties collection for member
            match properties.TryGetValue name with            
            | true,_ -> properties.[name] <- value
            | _      -> properties.Add(name,value)

    member this.Remove name =
        match ReflectionUtils.removeProperty this name with
        | true -> true
        // Maybe in map
        | false -> properties.Remove(name)


    override this.TryGetMember(binder:GetMemberBinder,result:obj byref ) =     
        match this.TryGetValue binder.Name with
        | Some value -> result <- value; true
        | None -> false


    override this.TrySetMember(binder:SetMemberBinder, value:obj) =        
        this.SetValue(binder.Name,value)
        true

    /// Returns both instance and dynamic properties when passed true, only dynamic properties otherwise. 
    /// Properties are returned as a key value pair of the member names and the boxed values
    member this.GetProperties includeInstanceProperties =        
        seq [
            if includeInstanceProperties then                
                for prop in ReflectionUtils.getPublicProperties (this.GetType()) -> 
                    new KeyValuePair<string, obj>(prop.Name, prop.GetValue(this, null))
            for key in properties.Keys ->
               new KeyValuePair<string, obj>(key, properties.[key]);
        ]

    /// Returns both instance and dynamic member names.
    /// Important to return both so JSON serialization with Json.NET works.
    override this.GetDynamicMemberNames() =
        this.GetProperties(true) |> Seq.map (fun pair -> pair.Key)

    /// Operator to access a dynamic member by name
    static member (?) (lookup:#DynamicObj,name:string) =
        match lookup.TryGetValue name with
        | Some(value) -> value
        | None -> raise <| System.MemberAccessException()        

    /// Operator to set a dynamic member
    static member (?<-) (lookup:#DynamicObj,name:string,value:'v) =
        lookup.SetValue (name,value)
    
    /// Copies all dynamic members of the DynamicObj to the target DynamicObj.
    member this.CopyDynamicPropertiesTo(target:#DynamicObj) =
        this.GetProperties(false)
        |> Seq.iter (fun kv ->
            target?(kv.Key) <- kv.Value
        )

    /// Returns a new DynamicObj with only the dynamic properties of the original DynamicObj (sans instance properties).
    member this.CopyDynamicProperties() =
        let target = DynamicObj()
        this.CopyDynamicPropertiesTo(target)
        target

    static member GetValue (lookup:DynamicObj,name) =
        lookup.TryGetValue(name).Value

    static member Remove (lookup:DynamicObj,name) =
        lookup.Remove(name)

    override this.Equals o =
        match o with
        | :? DynamicObj as other ->
            let subdictOf (super : Dictionary<'a, 'b>) (dict : Dictionary<'a, 'b>) =
                dict
                |> Seq.forall (fun pair ->
                    let (contains, value) = super.TryGetValue pair.Key
                    contains && value.Equals(pair.Value))
            subdictOf this.Properties other.Properties
        | _ -> false

    override this.GetHashCode () =
        this.Properties
        |> List.ofSeq
        |> List.sortBy (fun pair -> pair.Key)
        |> List.map (fun pair -> struct (pair.Key, pair.Value))
        |> (fun l -> l.GetHashCode())

    member this.ToJson () = JsonConvert.SerializeObject(this, new DynamicObjConverter())


module DynamicObj =

    /// <summary>
    /// Parse a DynamicObj to a json string.
    /// </summary>
    /// <param name="dynObj">The DynamicObj to parse to json.</param>
    let toJson (dynObj:DynamicObj) = JsonConvert.SerializeObject(dynObj, new DynamicObjConverter())

    /// <summary>
    /// Read json string to DynamicObj. If json string has an array as root element the array will be set as value with "root" as key.
    /// </summary>
    /// <param name="jsonSource">The json string to parse to a DynamicObj.</param>
    let ofJson (jsonSource:string) = JsonConvert.DeserializeObject<DynamicObj>(jsonSource, new DynamicObjConverter())


type private DynamicObjConverter() =
    inherit JsonConverter<DynamicObj>()

    override this.ReadJson(reader : JsonReader, objectType : System.Type, existingValue : DynamicObj, hasExistingValue:bool, serializer : JsonSerializer) : DynamicObj = 
       
        /// The isInit parameter is necessary as the reader starts with the first value.
        /// But every iteration thereafter we need to progress the reader to the next value, with reader.next().
        let rec readJsonParserFieldToDynObj (result: obj option) (isInit:bool) =
            let addValueToParentList(listObj:obj option) (value:'a) =
                // unbox 'a does not seem to provide any benefit. When comparing output to manually created dyn object,
                // it still needs to be boxed to be equal.
                let list = listObj.Value :?> obj seq |> Seq.map (fun x -> unbox<'a> x) |> List.ofSeq
                let res = (value::list) |> Seq.ofList |> box
                readJsonParserFieldToDynObj (Some res) false
            let next = isInit || reader.Read()
            if next = false then 
                result
            else 
                let isList = result.IsSome && result.Value :? obj seq
                let tokenType = reader.TokenType
                let tokenValue = (if isNull reader.Value then None else string reader.Value |> Some)
                //printfn "%A, %A" tokenType tokenValue
                match tokenType with
                | JsonToken.StartObject ->
                    let obj = DynamicObj() |> box
                    if isList then
                        let v = readJsonParserFieldToDynObj (Some obj) false
                        addValueToParentList result v.Value
                    else
                        readJsonParserFieldToDynObj (Some obj) false
                | JsonToken.EndObject -> 
                    result
                | JsonToken.StartArray ->
                    // Need to use Sequence to be able to use any casting to and from: obj seq <-> 'a seq
                    let list: obj seq = Seq.empty
                    readJsonParserFieldToDynObj (Some <| box list) false
                | JsonToken.EndArray ->
                    let list = result.Value :?> obj seq |> List.ofSeq |> List.rev
                    Some <| box list
                | JsonToken.PropertyName ->
                    let key = tokenValue.Value
                    if result.IsNone then failwith "Cannot apply property without parent dyn object."
                    let parent = 
                        match result.Value with
                        | :? DynamicObj ->
                            let logger = result.Value :?> DynamicObj
                            let v = readJsonParserFieldToDynObj None false
                            logger.SetValue(key, v.Value)
                            logger |> box
                        | _ -> failwith "Cannot parse parent type to supported types." 
                    readJsonParserFieldToDynObj (Some parent) false
                | JsonToken.String -> 
                    let v = string tokenValue.Value
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | JsonToken.Integer -> 
                    let v = int tokenValue.Value
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | JsonToken.Float -> 
                    let v = float tokenValue.Value
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | JsonToken.Boolean ->
                    let v = System.Boolean.Parse tokenValue.Value
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | JsonToken.Null ->
                    let v = None
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | JsonToken.Bytes | JsonToken.Date ->
                    let v = string tokenValue.Value
                    if isList then
                        addValueToParentList result v
                    else
                        Some <| box v
                | any -> 
                    readJsonParserFieldToDynObj None false
        let res = readJsonParserFieldToDynObj(None) true |> Option.get
        match res with
        | :? list<obj> as list ->
            let loggerList = list
            let r = DynamicObj()
            r.SetValue("root", loggerList)
            r
        | :? DynamicObj as root ->
            root
        | _ -> failwith "Could not parse Result to any supported type."

    override this.WriteJson(writer : JsonWriter, value : DynamicObj, serializer : JsonSerializer) =
        let v =
            let settings = 
                let s = JsonSerializerSettings()
                s.ReferenceLoopHandling <- ReferenceLoopHandling.Serialize
                s
            let hasRootArr = value.TryGetValue "root"
            if hasRootArr.IsSome then
                hasRootArr.Value 
                |> fun v -> JsonConvert.SerializeObject(v, settings)
            else
                JsonConvert.SerializeObject(value, settings)
        writer.WriteRaw (v)
