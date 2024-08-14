namespace DynamicObj

open System.Reflection

type PropertyHelper =

    {
        Name : string
        IsStatic : bool
        IsDynamic : bool
        IsMutable : bool
        IsImmutable : bool
        GetValue : obj -> obj
        SetValue : obj -> obj -> unit
        RemoveValue : obj -> unit
    }
    
    #if !FABLE_COMPILER

    static member fromPropertyInfo (pI : PropertyInfo) =
        {
            Name = pI.Name
            IsStatic = true
            IsDynamic = false
            IsMutable = pI.CanWrite
            IsImmutable = not pI.CanWrite
            GetValue = fun(o) -> pI.GetValue(o)
            SetValue = fun o v -> pI.SetValue(o, v)
            RemoveValue = fun o -> pI.SetValue(o, null)
        }

    #endif

