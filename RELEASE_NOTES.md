### 3.1.0+87113a4 (Released 2024-9-2)
* Additions:
    * [[#e434c16](https://github.com/CSBiology/DynamicObj/commit/e434c162459b5c163bd387ef8b6aae4fbe6422cf)] add typed property retrieval function
    * [[#ebfe3a7](https://github.com/CSBiology/DynamicObj/commit/ebfe3a79919a3be02472cef0d87e781bc776ff7c)] small changes according to PR
    * [[#8f6958f](https://github.com/CSBiology/DynamicObj/commit/8f6958f859cebbbd695748ee32092ea20e69b325)] add copyDynamicProperties
    * [[#1ed4f9d](https://github.com/CSBiology/DynamicObj/commit/1ed4f9d84abc26918739eac59879e0f6d9ba27e9)] create unified API for static and dynamic property accession
    * [[#9870af8](https://github.com/CSBiology/DynamicObj/commit/9870af8e0f1f658c0d70321325efa83087d5c81e)] Create manage-issues.yml

* * Deletions:
    * [[#ab21626](https://github.com/CSBiology/DynamicObj/commit/ab2162606334cded61363e6b7006444d0525c5d4)] remove TryGetTypedValue member from transpilation
    * [[#745997a](https://github.com/CSBiology/DynamicObj/commit/745997a1d6239ae3a668bc3d61771dfd77899bc4)] remove poetry
* Bugfixes:
    * [[#2bef816](https://github.com/CSBiology/DynamicObj/commit/2bef816e62925dd8aad6aaae7351c99c06769574)] fix python encoding issue
    * [[#8fc4f25](https://github.com/CSBiology/DynamicObj/commit/8fc4f2552bd87f50a97c3c06b31ab3d14ebb687a)] fix python non-exisiting member accession

### 3.0.0+ef44100 (Released 2024-9-2)
    * - Make project fable-compatible
    * - Rework project structure by splitting into DynamicObj and DynamicObj.Immutable
    * - Add a bunch of tests
    * - Backwards incompatible because of renaming and removing some members

### 2.0.0 (Released 2024-9-2)
    * - [Use a strong name for the assembly](https://github.com/CSBiology/DynamicObj/pull/19). This may cause backwards incompatibility on netfx. (thanks [@WhiteBlackGoose](https://github.com/WhiteBlackGoose))

### 1.0.1 (Released 2024-9-2)
    * Fix up some TFM confusions

### 1.0.0 (Released 2024-9-2)
    * - [Rename IDO combine to `combineWith`, which now also preserves the type of the second IDO.](https://github.com/CSBiology/DynamicObj/pull/12/files) (thanks [@WhiteBlackGoose](https://github.com/WhiteBlackGoose))
    * - Target .NET 6

### 0.2.1 (Released 2024-9-2)
    * - Add `combine` for IDO

### 0.2.0 (Released 2024-9-2)
    * - [Add `ImmutableDynamicObj`](https://github.com/CSBiology/DynamicObj/pull/4) as the immutable counterpart of `DynamicObj` (thanks [@WhiteBlackGoose](https://github.com/WhiteBlackGoose))
    * - Add print formatters for both `DynamicObj` and `ImmutableDynamicObj`
    * - [Enable Json serialization for ImmutableDynamicObj](https://github.com/CSBiology/DynamicObj/commit/e7474d2658a234bb94299f12de30625e04f5f407) (thanks [@WhiteBlackGoose](https://github.com/WhiteBlackGoose))

### 0.1.0 (Released 2024-9-2)
    * - target netstandard2.0 and .net5.0
    * - add custom Equality and GetHashcode (thanks [@WhiteBlackGoose](https://github.com/WhiteBlackGoose))
    * - publish symbols

### 0.0.3 (Released 2024-9-2)
    * - target netstandard2.0

### 0.0.2 (Released 2024-9-2)
    * - Add copy utils

### 0.0.1 (Released 2024-9-2)
    * initial release

