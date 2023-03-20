# InspectorOnlyFields

[![openupm](https://img.shields.io/npm/v/com.github.miyaji255.inspector-only-fields?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.github.miyaji255.inspector-only-fields/) [![CI](https://github.com/miyaji255/InspectorOnlyFields/actions/workflows/analyzer-ci.yml/badge.svg?branch=main)](https://github.com/miyaji255/InspectorOnlyFields/actions/workflows/analyzer-ci.yml)


[English](README.md) / Japanese

このパッケージはUnityのインスペクター上から代入するフィールドに、C#コード上で代入することを警告するアナライザーを提供します。

## インストール方法

このパッケージは UPM(Unity Package Manager) を利用してインストールすることができます。

### Git URL を使用する方法

1. **Package Manager** ウィンドウを開く
2. ステータスバーの **Add**(+) をクリック
3. **Add package from git URL** を選択
4. `https://github.com/miyaji255/InspectorOnlyFields.git?path=InspectorOnlyFields/Packages/InspectorOnlyFields` を入力
5. **Add** をクリック

### OpenUPM を使用する方法

こちらのページからインストールすることができます
https://openupm.com/packages/com.github.miyaji255.inspector-only-fields/

## 使い方

インスペクターから値を渡すフィールドに`InspectorOnly`属性をつけます。すると、代入時に警告が出されるようになります。
```csharp
using InspectorOnlyFields;
using UnityEngine;

public class SampleObject : MonoBehaviour
{
    
    // InspOnly001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
    [InspectorOnly]
    public GameObject GameObject = new GameObject();

    void Start()
    {
        // InspOnly001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
        GameObject = new GameObject();
    }
}
```
