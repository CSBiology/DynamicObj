// For more information see https://aka.ms/fsharp-console-apps

open DynamicObj

type Inner() =
    inherit DynamicObj()
    static member init(
        ?inner_value: string
    ) =
        Inner()
        |> DynObj.withOptionalProperty "inner_value" inner_value

type Outer() =
    inherit DynamicObj()
    static member init(
        ?A: int,
        ?B: string,
        ?Inner: Inner
    ) =
        Outer()
        |> DynObj.withOptionalProperty "A" A
        |> DynObj.withOptionalProperty "B" B
        |> DynObj.withOptionalProperty "Inner" Inner


let outer1 = Outer.init(A = 1, B = "first", Inner = Inner.init(inner_value = "inner_first"))
let outer2 = Outer.init(A = 2, B = "second", Inner = Inner.init(inner_value = "inner_second"))
let expected = Outer.init(A = 2, B = "second", Inner = Inner.init(inner_value = "inner_second"))

printfn "%A" ((DynObj.combine outer1 outer2) = expected)