namespace DynamicObj

open System.Collections.Generic

/// <summary>
/// This module contains lots of API functions for DynamicObj. 
///
/// These functions are not static methods on the DynamicObj type itself because that type is designed to be inherited from, 
/// and a lot of these functions might not make sense as static methods on inheriting types.
/// </summary>
module DynObj =

    /// <summary>
    /// Creates a new DynamicObj from a Dictionary containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties">The dictionary with the dynamic properties</param>
    let ofDict (dynamicProperties: Dictionary<string, obj>) = DynamicObj.ofDict dynamicProperties

    /// <summary>
    /// Creates a new DynamicObj from a sequence of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofSeq (dynamicProperties: seq<string * obj>) = 
        dynamicProperties
        |> dict
        |> Dictionary
        |> DynamicObj.ofDict

    /// <summary>
    /// Creates a new DynamicObj from a list of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofList (dynamicProperties: (string * obj) list) = 
        dynamicProperties
        |> ofSeq

    /// <summary>
    /// Creates a new DynamicObj from an array of key value pairs containing dynamic properties.
    /// </summary>
    /// <param name="dynamicProperties"></param>
    let ofArray (dynamicProperties: (string * obj) array) = 
        dynamicProperties
        |> ofSeq

    /// <summary>
    /// Combines the dynamic properties of the second DynamicObj onto the first. 
    ///
    /// In case of duplicate property names the members of the second object override those of the first.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <remarks>This function mutates the first input DynamicObj</remarks>
    let rec combine (first:DynamicObj) (second:DynamicObj) =
        //printfn "Type %A" (first.GetType())
        /// Consider deep-copy of first
        for kv in (second.GetProperties true) do 
            match kv.Value with
            | :? DynamicObj as valueS -> 
                // is dynObj in second
                match first.TryGetPropertyValue (kv.Key) with
                | Some valueF -> 
                    let tmp = combine (unbox valueF) (unbox valueS)
                    first.SetProperty(kv.Key,tmp)
                | None -> first.SetProperty(kv.Key,valueS)
            | _ -> first.SetProperty(kv.Key,kv.Value)
        first

    /// <summary>
    /// Returns Some('TPropertyValue) when a dynamic (or static) property with the given name and type exists on the input, otherwise None.
    /// </summary>
    /// <param name="propertyName">the name of the property to get</param>
    /// <param name="dynObj">the input DynamicObj</param>
    let inline tryGetTypedPropertyValue<'TPropertyValue> (propertyName:string) (dynObj : DynamicObj) : 'TPropertyValue option =
        match (dynObj.TryGetPropertyValue propertyName) with
        | None -> None
        | Some o -> 
            match o with
            | :? 'TPropertyValue as o -> o |> Some
            | _ -> None

    /// <summary>
    /// Sets the dynamic (or static) property value with the given name on the given DynamicObj, creating a new dynamic property if none exists.
    /// </summary>
    /// <param name="propertyName">The name of the dynamic property to set</param>
    /// <param name="propertyValue">The value of the dynamic property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setProperty (propertyName:string) (propertyValue:'TPropertyValue) (dynObj : DynamicObj) =
        dynObj.SetProperty(propertyName,propertyValue)

    /// <summary>
    /// Sets the dynamic (or static) property value with the given name, creating a new dynamic property if none exists on the given DynamicObj and returns it.
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="propertyValue">The value of the property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let inline withProperty (propertyName:string) (propertyValue:'TPropertyValue) (dynObj: #DynamicObj) =
        setProperty propertyName propertyValue dynObj
        dynObj

    /// <summary>
    /// Sets the dynamic (or static) property value with the given name on the given DynamicObj if the value is Some('TPropertyValue), creating a new dynamic property if none exists.
    /// If the given propertyValue is None, does nothing to the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="propertyValue">The value of the property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setOptionalProperty (propertyName: string) (propertyValue: 'TPropertyValue option) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> setProperty propertyName pv
        | None -> ()

    /// <summary>
    /// Sets the dynamic (or static) property value with the given name on the given DynamicObj if the value is Some('TPropertyValue), creating a new dynamic property if none exists, and returns it.
    /// If the given propertyValue is None, does nothing to the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="propertyValue">The value of the property to set</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let inline withOptionalProperty (propertyName: string) (propertyValue: 'TPropertyValue option) (dynObj: #DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> withProperty propertyName pv
        | None -> dynObj

    /// <summary>
    /// Sets the given dynamic (or static) property with the result of a mapping function applied to the given property value on the given DynamicObj if the value is Some('TPropertyValue).
    /// If the given propertyValue is None, does nothing to the input DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="propertyValue">The value of the property to set</param>
    /// <param name="mapping">A function to apply to the property value before setting it on the DynamicObj</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let setOptionalPropertyBy (propertyName: string) (propertyValue: 'TPropertyValue option) (mapping: 'TPropertyValue -> 'UPropertyValue) (dynObj: DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> setProperty propertyName (mapping pv)
        | None -> ()

    /// <summary>
    /// Sets the given dynamic (or static) property with the result of a mapping function applied to the given property value on the given DynamicObj if the value is Some('TPropertyValue) and returns it.
    /// If the given propertyValue is None, returns the unchanged DynamicObj.
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="propertyValue">The value of the property to set</param>
    /// <param name="mapping">A function to apply to the property value before setting it on the DynamicObj</param>
    /// <param name="dynObj">The DynamicObj to set the property on</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    let inline withOptionalPropertyBy (propertyName: string) (propertyValue: 'TPropertyValue option) (mapping: 'TPropertyValue -> 'UPropertyValue) (dynObj: #DynamicObj) = 
        match propertyValue with
        | Some pv -> dynObj |> withProperty propertyName (mapping pv)
        | None -> dynObj

    /// <summary>
    /// Returns Some(boxed property value) if a dynamic (or static) property with the given name exists on the input, otherwise None.
    /// </summary>
    /// <param name="propertyName">The name of the property to get</param>
    /// <param name="dynObj">The DynamicObj to get the property from</param>
    let tryGetPropertyValue (propertyName: string) (dynObj: DynamicObj) = 
        dynObj.TryGetPropertyValue propertyName

    /// <summary>
    /// Removes any dynamic property with the given name from the input DynamicObj.
    /// If the property is static and mutable, it will be set to null.
    /// Static immutable properties cannot be removed.
    /// </summary>
    /// <param name="propertyName">The name of the property to remove</param>
    /// <param name="dynObj">The DynamicObj to remove the property from</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    /// <exception cref="System.MemberAccessException">Thrown if the dynamic property does not exist</exception>
    let removeProperty (propertyName: string) (dynObj: DynamicObj) = 
        dynObj.RemoveProperty(propertyName) |> ignore

    /// <summary>
    /// Removes any dynamic property with the given name from the input DynamicObj and returns it.
    /// If the property is static and mutable, it will be set to null.
    /// Static immutable properties cannot be removed.
    /// </summary>
    /// <param name="propertyName">The name of the property to remove</param>
    /// <param name="dynObj">The DynamicObj to remove the property from</param>
    /// <remarks>This function mutates the input DynamicObj</remarks>
    /// <exception cref="System.MemberAccessException">Thrown if the dynamic property does not exist</exception>
    let inline withoutProperty(propertyName: string) (dynObj: #DynamicObj) = 
        dynObj |> removeProperty propertyName
        dynObj

    /// <summary>
    /// Returns a formatted string containing all static and dynamic properties of the given DynamicObj
    /// </summary>
    /// <param name="dynObj">The DynamicObj for which to generate a formatted string for</param>
    let format (dynObj:DynamicObj) =
    
        let members = dynObj.GetPropertyHelpers(true) |> List.ofSeq

        let rec loop (object:DynamicObj) (indentationLevel:int) (membersLeft:PropertyHelper list) (acc:string list) =
            let indent = [for i in 0 .. indentationLevel-1 do yield "    "] |> String.concat ""
            match membersLeft with
            | [] -> acc |> List.rev |> String.concat System.Environment.NewLine
            | m::rest ->
                let item = m.GetValue object
                let dynamicIndicator = if m.IsDynamic then "?" else ""
                let name = m.Name
                match item with
                | :? DynamicObj as item -> 
                    let innerMembers = item.GetPropertyHelpers(true) |> List.ofSeq
                    let innerPrint = (loop item (indentationLevel + 1) innerMembers [])              
                    loop object indentationLevel rest ($"{indent}{dynamicIndicator}{name}:{System.Environment.NewLine}{innerPrint}" :: acc)
                | item -> 
                    loop object indentationLevel rest ($"{indent}{dynamicIndicator}{name}: {item}"::acc)
    
        loop dynObj 0 members []

    /// <summary>
    /// Prints a formatted string containing all static and dynamic properties of the given DynamicObj
    /// </summary>
    /// <param name="dynObj">The DynamicObj for which to print a formatted string for</param>
    let print (dynObj:DynamicObj) = printfn "%s" (dynObj |> format)

    /// <summary>
    /// function to deep copy a boxed object (if possible)
    ///
    /// The following cases are handled (in this precedence):
    ///
    /// - Basic F# types (`bool`, `byte`, `sbyte`, `int16`, `uint16`, `int`, `uint`, `int64`, `uint64`, `nativeint`, `unativeint`, `float`, `float32`, `char`, `string`, `unit`, `decimal`)
    /// 
    /// - `ResizeArrays` and `Dictionaries` containing any combination of basic F# types
    /// 
    /// - `Dictionaries` containing `DynamicObj` as keys or values in any combination with `DynamicObj` or basic F# types as keys or values
    /// 
    /// - `array<DynamicObj>`, `list<DynamicObj>`, `ResizeArray<DynamicObj>`: These collections of DynamicObj are copied as a new collection with recursively deep copied elements.
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
    /// <param name="includeInstanceProperties">Whether to include instance properties (= 'static' properties on the class) as dynamic properties on the new instance for matched DynamicObj.</param>
    let tryDeepCopyObj (includeInstanceProperties:bool) (o:DynamicObj) =
        CopyUtils.tryDeepCopyObj(o, includeInstanceProperties)