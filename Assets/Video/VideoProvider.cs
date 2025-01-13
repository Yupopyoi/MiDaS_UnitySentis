// Copyright (c) 2025 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ML_Runner.Source
{
    public class VideoProvider : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] RawImage _inputRawImage;
        [SerializeField] int _initialCamera = 0;

        [Header("UI")]
        [SerializeField] TMP_Dropdown _cameraDropdown;

        private WebCamTexture _webCamTexture;

        // Camera List
        private WebCamDevice[] _devices;

        public Texture CameraTexture => _webCamTexture;

        void Start()
        {
            _devices = WebCamTexture.devices;

            if (_devices.Length == 0)
            {
                Debug.LogError("No Camera Found!");
                return;
            }

            if (_cameraDropdown != null)
            {
                _cameraDropdown.options.Clear();
                foreach (var device in _devices)
                {
                    _cameraDropdown.options.Add(new TMP_Dropdown.OptionData(device.name));
                }

                _cameraDropdown.value = 100;
                _cameraDropdown.onValueChanged.AddListener(index => SetCamera(index));
            }

            // Set the Initial Camera
            if (_initialCamera >= _devices.Length)
            {
                Debug.LogError("No camera exists with the selected number!");
                return;
            }
            _cameraDropdown.value = _initialCamera;
        }

        public void SetCamera(int cameraIndex)
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }

            _webCamTexture = new WebCamTexture(_devices[cameraIndex].name);
            _webCamTexture.Play();

            if(_inputRawImage != null)
            {
                _inputRawImage.texture = _webCamTexture;
                _inputRawImage.material.mainTexture = _webCamTexture;
            }
        }

        void OnDestroy()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }
        }
    }
} // namespace ML_Runner.Input
