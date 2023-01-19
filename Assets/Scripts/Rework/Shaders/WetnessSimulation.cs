namespace TideDefense
{
    using System.Collections;
    using System.Collections.Generic;
	using DG.Tweening;
	using UnityEngine;
    using UnityEngine.Rendering;

    public class WetnessSimulation : MonoBehaviour
    {
        public Texture initialTexture = null;
        public RenderTexture texture = null;

        [HideInInspector]
        public RenderTexture buffer = null;
        public Material material = null;

        private float[] _textureCoveragePerSegment = new float[12];

        [SerializeField]
        private float _updateFrequency = 0.5f;

        public IEnumerator UpdateTextureBehaviour()
        {
            while (true)
            {
                yield return new WaitForSeconds(_updateFrequency);
                yield return new WaitForFixedUpdate();
                UpdateTexture();
                yield return null;
            }
        }

        public void Start()
        {
            Graphics.Blit(initialTexture, texture);
            buffer = new RenderTexture(
                texture.width,
                texture.height,
                texture.depth,
                texture.format
            );

            // WaveSimulation();

            StartCoroutine("UpdateTextureBehaviour");
        }

        public const string SEGMENTS_PROPERTY = "_SegmentsProgress";

        public void RefreshTextureCoverage(float[] textureCoveragePerSegment)
        {
            _textureCoveragePerSegment = textureCoveragePerSegment;
            material.SetFloatArray(SEGMENTS_PROPERTY, _textureCoveragePerSegment);
        }


        // public void WaveSimulation()
        // {
        //     _values = new List<float>(2){0, 0};
        //     Debug.Log(_values.Count);
        //     DOVirtual
        //         .Float(
        //             0f,
        //             Mathf.PI,
        //             5,
        //             (float value) =>
        //             {
        //                 _values[0] = Mathf.Lerp(0f, 1f, Mathf.Sin(value));
        //                 Debug.Log(_values[0]);
        //                 material.SetFloatArray(SEGMENTS_PROPERTY, _values);
        //             }
        //         )
        //         .SetEase(Ease.InOutSine)
        //         .SetLoops(-1);
        // }

        public void UpdateTexture()
        {
            Graphics.Blit(texture, buffer, material);
            Graphics.Blit(buffer, texture);
        }
    }
}
