module DynamicObjs.Tests

open Fable.Pyxpecto
open DynamicObj.Tests

let main = testList "DynamicObj (Class)" [

    GetHashCode.tests_GetHashCode
    Equals.tests_Equals

    SetProperty.tests_SetProperty
    RemoveProperty.tests_RemoveProperty
    
    TryGetPropertyValue.tests_TryGetPropertyValue
    GetPropertyValue.tests_GetPropertyValue

    #if !FABLE_COMPILER
    // instance method TryGetTypedValue is not Fable-compatible
    TryGetTypedPropertyValue.tests_TryGetTypedPropertyValue
    #endif

    TryGetStaticPropertyHelper.tests_TryGetStaticPropertyHelper
    TryGetDynamicPropertyHelper.tests_TryGetDynamicPropertyHelper
    TryGetPropertyHelper.tests_TryGetPropertyHelper
    GetPropertyHelpers.tests_GetPropertyHelpers
    GetProperties.tests_GetProperties

    ShallowCopyDynamicPropertiesTo.tests_ShallowCopyDynamicPropertiesTo
    ShallowCopyDynamicProperties.tests_ShallowCopyDynamicProperties
    DeepCopyDynamicProperties.tests_DeepCopyDynamicProperties
]