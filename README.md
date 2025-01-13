# MiDaS_UnitySentis

ToDo : 画像を貼る:running:  

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

Now all you have to do is play the Unity project.

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

あとは、Unityプロジェクトを再生するだけです。

### Qiita記事

:construction: 執筆中 :construction:
