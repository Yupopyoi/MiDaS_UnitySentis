// Copyright (c) 2025 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;
using Unity.Sentis;

namespace ML_Runner
{
    public class Executor : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] Source.VideoProvider _textureProvider;
        [SerializeField] ModelAsset _modelAsset;
        [SerializeField] int _inputWidth = 384;

        Tensor<float> _input;
        Model _runtimeModel;
        Worker _worker;

        bool _inferencePending = false;
        bool _isTaskRunning = true;

        [Header("Output")]
        [SerializeField] int _outputWidth = 384;
        Tensor<float> _rawOutput;
        Tensor<float> _output;
        RenderTexture _outputTexture;

        // PostProcess
        [Header("PostProcess")]
        [SerializeField] ComputeShader _postProcessComputeShader;
        GraphicsBuffer _inputBuffer;
        GraphicsBuffer _outputBuffer;
        int _kernel;

        public UnityEvent<RenderTexture> OnInferenceCompleted;

        void OnEnable()
        {
            _isTaskRunning = true;

            _runtimeModel = ModelLoader.Load(_modelAsset);
            _worker = new Worker(_runtimeModel, BackendType.GPUCompute);

            // Allocate memory for Tensor / ComputeBuffer in advance.
            _input     = new Tensor<float>(new TensorShape(1, 3 /* RGB */         , _inputWidth, _inputWidth));
            _rawOutput = new Tensor<float>(new TensorShape(1, 1 /* Depth Only  */ , _inputWidth, _inputWidth));
            _output    = new Tensor<float>(new TensorShape(1, 4 /* RGBA */        , 384, 384));

            var type = GraphicsBuffer.Target.Index | GraphicsBuffer.Target.Raw;
            _inputBuffer  = new GraphicsBuffer(type, _outputWidth * _outputWidth, sizeof(float));
            _outputBuffer = new GraphicsBuffer(type, _outputWidth * _outputWidth * 4 /* RGBA */, sizeof(float));

            // Compute shader configuration.
            _kernel = _postProcessComputeShader.FindKernel("CSMain");
            _postProcessComputeShader.SetBuffer(_kernel, "DepthInput", _inputBuffer);
            _postProcessComputeShader.SetBuffer(_kernel, "ColorOutput", _outputBuffer);
            _postProcessComputeShader.SetInt("InputDataNum", _outputWidth * _outputWidth);
            _postProcessComputeShader.SetFloat("MinDepth", 0f);
            _postProcessComputeShader.SetFloat("MaxDepth", 4096f);

            // Output RenderTexture
            _outputTexture = new(_outputWidth, _outputWidth, 0, RenderTextureFormat.ARGBFloat)
            {
                enableRandomWrite = true
            };
        }

        void Update()
        {
            if (!_inferencePending)
            {
                _inferencePending = true;

                TextureConverter.ToTensor(_textureProvider.CameraTexture, _input, new TextureTransform());
                
                // Execute on the worker.(Non-blocking)
                _worker.Schedule(_input);
                _rawOutput = _worker.PeekOutput() as Tensor<float>;

                // Return a copy of the result to the CPU asynchronously.
                var awaiter = _rawOutput.ReadbackAndCloneAsync().GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    if (!_isTaskRunning) return;

                    Tensor<float> tensorOut = awaiter.GetResult();

                    // Use this Debug.Log if you want to know the size of the output.
                    // Debug.Log(string.Format("Output : ({0},{1},{2})" ,tensorOut.shape[0], tensorOut.shape[1], tensorOut.shape[2]));

                    tensorOut.Reshape(new TensorShape(1, tensorOut.shape[0], tensorOut.shape[1], tensorOut.shape[2]));
                   
                    ConvertToDepthMap(tensorOut);

                    _outputTexture = TextureConverter.ToTexture(_output, _outputWidth, _outputWidth, 4  /* Channels (RGBA) */);
                    
                    tensorOut.Dispose();

                    OnInferenceCompleted?.Invoke(_outputTexture);
                    
                    _inferencePending = false;
                });
            }
        }

        /// <summary>
        /// Convert the resulting tensor to a depth map.
        /// This function modifies _output directly.
        /// </summary>
        /// <param name="output"></param>
        void ConvertToDepthMap(Tensor<float> output)
        {
            // Set data in input buffer.
            NativeArray<float> nativeOutput = output.DownloadToNativeArray();

            // Use this Debug.Log if you want to know the size of the output.
            // Debug.Log(nativeOutput.Length);

            _inputBuffer.SetData(nativeOutput);

            // Execute the compute shader
            _postProcessComputeShader.Dispatch(_kernel, _outputWidth / 8, _outputWidth / 8, 1);

            float[] outputData = new float[_outputWidth * _outputWidth * 4];
            _outputBuffer.GetData(outputData);

            // Upload to Tensor<float> _output
            _output.Upload(outputData);

            nativeOutput.Dispose();
        }

        void OnDestroy()
        {
            _isTaskRunning = false;

            // [ToDo] Solve "Found unreferenced, but undisposed CPUTensorData which might lead to CPU resource leak"
            _worker?.Dispose();

            _input?.Dispose();
            _rawOutput?.Dispose();
            _output?.Dispose();

            _inputBuffer?.Release();
            _outputBuffer?.Release();

            if (_outputTexture != null) Destroy(_outputTexture);
        }
    }
}
// namespace ML_Runner
