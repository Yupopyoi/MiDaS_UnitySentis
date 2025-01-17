# MiDaS_UnitySentis

![Demo Image](https://github.com/Yupopyoi/MiDaS_UnitySentis/blob/main/DemoImage/MiDaS_DemoImage.png)

## English Description

This is a sample code to execute depth estimation using MiDaS with Unity.Sentis.

> [!IMPORTANT]
> GPU usage is assumed and compute shader is used for post-processing (depth mapping).

### Development environment

Unity : 6000.0.31f1  
OS : Windows 11  
GPU : GeForce RTX 4070 Ti  

Unity.Sentis 2.1.1  

### How To Use

#### Get MiDaS Model(s)

:warning: **This repository does not include MiDaS models**  

Get any onnx format model from [here](https://github.com/isl-org/MiDaS/releases/tag/v2_1).  

> [!NOTE]
> Operation is checked with ```model-f6b98070.onnx```

Please put the downloaded onnx file in Assets folder of Unity.

#### Execution

Register the model you just created in the ModelAsset of the Exector (component) attached to the Exector (object).  

![RegisterModel](https://github.com/Yupopyoi/MiDaS_UnitySentis/blob/main/DemoImage/RegisterModel.png)

Now all you have to do is play the Unity project. ✨

### Other Model?

You can use other depth estimation models besides MiDaS by simply changing the settings.  

#### Depth Anything V2

Get the model in onnx format on [this page](https://github.com/fabio-sim/Depth-Anything-ONNX/releases/tag/v2.0.0).

:construction: **Some models are not available.** Under investigation...  :construction:  

I confirmed that it works correctly when using  ```depth_anything_v2_vitb_indoor_dynamic.onnx```.  

The value of Input Width and Output Width should be **518**.

![Depth Anything](https://github.com/Yupopyoi/MiDaS_UnitySentis/blob/main/DemoImage/DepthAnything.png)

## 日本語説明

MiDaSを利用した深度推定を、Unity.Sentisを用いて実行するサンプルプログラムです。

> [!IMPORTANT]
> GPU使用を前提とし、後処理（深度マップ作成）にはコンピュートシェーダを用いています。

### 使用方法

#### MiDaS モデルの入手

:warning: **このレポジトリに、MiDaSモデルは含まれていません。**  

[ここ](https://github.com/isl-org/MiDaS/releases/tag/v2_1)から、任意のonnx形式モデルを入手してください。  

> [!NOTE]
> なお、動作確認は、```model-f6b98070.onnx``` を用いています。  

ダウンロードしたonnxファイルは、UnityのAssets内においてください。

#### 実行

Exector (オブジェクト)にアタッチされている Exector（コンポーネント）の、ModelAssetに先ほどのモデルを登録してください。

![RegisterModel](https://github.com/Yupopyoi/MiDaS_UnitySentis/blob/main/DemoImage/RegisterModel.png)  

あとは、Unityプロジェクトを再生するだけです。

### 他のモデルを使用する

MiDaS以外の深度推定モデルでも、設定を変更することで使用できます。  

#### Depth Anything V2

[ここ](https://github.com/fabio-sim/Depth-Anything-ONNX/releases/tag/v2.0.0) から onnx 形式のモデルを入手します。  
Sentisが対応していない部分もあるようで、使用できないモデルもあるようです。  
とはいえ、最低限 ```depth_anything_v2_vitb_indoor_dynamic.onnx``` は使用できることを確認済みです。  
なお、入力・出力サイズ（Input Width と Output Width）は 518 に変更してください。  

### Qiita記事

:construction: 執筆中 :construction:
