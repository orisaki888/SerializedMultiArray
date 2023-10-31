# SerializedMultiArray
2次元配列をシリアライズして、Unityのインスペクター上に表示するスクリプトです。  
Bool型だけに対応したBoolPlaneクラスと複数の型に対応したSerializedPlaneクラスがあります。

![Imgur](https://i.imgur.com/Zg00gFH.png)
![Imgur](https://i.imgur.com/Yv8TE7D.png)


## 使用方法

使い方は単純で、通常の変数宣言をするだけです。  
SerializedPlaneは現在、以下の型に対応しています。
* int
* float
* string
* UnityEngine.Object
* GameObject
* Color

BoolPlaneの例:

```
  public BoolPlane BoolArray;
```

SerializedPlaneの例:

```
  public SerializedPlane<int> IntArray;
  public SerializedPlane<float> FloatArray;
  public SerializedPlane<string> StringArray;
  public SerializedPlane<Object> ObjectArray;
  public SerializedPlane<GameObject> GameObjectArray;
  public SerializedPlane<Color> ColorArray;
```

![Imgur](https://i.imgur.com/BQD8Wxp.png)
