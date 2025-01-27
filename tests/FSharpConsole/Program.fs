open DynamicObj

let constructDeepCopiedClone<'T> (props: seq<string*obj>) =
    let original = DynamicObj()
    props
    |> Seq.iter (fun (propertyName, propertyValue) -> original.SetProperty(propertyName, propertyValue))
    let clone : 'T = original.DeepCopyProperties() |> unbox<'T>
    original, clone 

let item1 = DynamicObj() |> DynObj.withProperty "item" 1
let item2 = DynamicObj() |> DynObj.withProperty "item" 2
let item3 = DynamicObj() |> DynObj.withProperty "item" 3
let arr = [|item1; item2; item3|]
let original, clone = constructDeepCopiedClone<DynamicObj> ["arr", box arr]
item1.SetProperty("item", -1)
item2.SetProperty("item", -1)
item3.SetProperty("item", -1)

original 
|> DynObj.tryGetTypedPropertyValue<DynamicObj []> "arr"
|> Option.iter (Seq.iter DynObj.print)

clone
|> DynObj.tryGetTypedPropertyValue<DynamicObj []> "arr"
|> Option.iter (Seq.iter DynObj.print)