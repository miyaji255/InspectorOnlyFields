# InspectorOnlyFields

Unityでシリアライズしたフィールドにコードから書き換えようとすると警告するパッケージです

## インストール方法

このパッケージは UPM(Unity Package Manager) を利用してインストールすることができます。

1. Package Manager ウィンドウを開く
2. ステータスバーの **Add**(+) をクリック
3. **Add package from git URL** を選択
4. `https://github.com/miyaji255/InspectorOnlyFields.git?path=InspectorOnlyFields/Packages/InspectorOnlyFields` を入力
5. **Add** をクリック

## 使い方

インスペクターから値を渡すフィールドに`InspectorOnly`属性をつけます。すると、代入時に警告が出されるようになります。
```csharp
using InspectorOnlyFields;
using UnityEngine;

public class SampleObject : MonoBehaviour
{
    
    // IO001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
    [InspectorOnly]
    public GameObject GameObject = new GameObject();

    void Start()
    {
        // IO001: 'GameObject' へ代入することは InspectorOnly 属性により禁止されています
        GameObject = new GameObject();
    }
}
```
