# InspectorOnlyFields

[![openupm](https://img.shields.io/npm/v/com.github.miyaji255.inspector-only-fields?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.github.miyaji255.inspector-only-fields/)

English / [Japanese](README_ja.md)

This package provides an analyzer that warns against assigning on C# code to fields that are assigned from within the Unity inspector.

## Installation

You can install this package using UPM (Unity Package Manager).

### Use Git URL
1. Open **Package Manager** Window
2. Click **Add**(+) on the status bar
3. Select **Add package from git URL**
4. Input `https://github.com/miyaji255/InspectorOnlyFields.git?path=InspectorOnlyFields/Packages/InspectorOnlyFields`
5. Click **Add**

### Use OpenUPM

You can install using OpenUPM. The page is here.
https://openupm.com/packages/com.github.miyaji255.inspector-only-fields/

## Usage

Add the `InspectorOnly` attribute to fields to which you want to assign values from the inspector. Then, The analyzer will warn you when assigning a value to the fields.

```csharp
using InspectorOnlyFields;
using UnityEngine;

public class SampleObject : MonoBehaviour
{
    
    // IOF001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
    [InspectorOnly]
    public GameObject GameObject = new GameObject();

    void Start()
    {
        // IOF001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
        GameObject = new GameObject();
    }
}
```

Translated by [DeepL.com](https://www.deepl.com/)
