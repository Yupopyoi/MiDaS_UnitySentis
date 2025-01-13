// Copyright (c) 2025 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;

namespace ML_Runner.Source
{
    public class VideoDisplayer : MonoBehaviour
    {
        [SerializeField] RawImage _displayRawImage;

        public void UpdateImage(RenderTexture sourceTexture)
        {
            RenderTexture.active = sourceTexture;

            Texture2D outputTexture2D = new(sourceTexture.width, sourceTexture.height);
            outputTexture2D.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);
            outputTexture2D.Apply();

            _displayRawImage.texture = outputTexture2D;
        }
    }
}// namespace ML_Runner.Source
