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

        [SerializeField]
        private float _evaporationSpeed = 0.1f;

        public const string EVAPORATION_SPEED_PROPERTY = "_EvaporationSpeed";

        public const string SEGMENTS_PROPERTY = "_SegmentsProgress";

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

            StartCoroutine("UpdateTextureBehaviour");

            material.SetFloat(EVAPORATION_SPEED_PROPERTY, _evaporationSpeed);
        }

        private void OnValidate()
        {
            material.SetFloat(EVAPORATION_SPEED_PROPERTY, _evaporationSpeed);
        }

        public void RefreshTextureCoverage(float[] textureCoveragePerSegment)
        {
            _textureCoveragePerSegment = textureCoveragePerSegment;
            material.SetFloatArray(SEGMENTS_PROPERTY, _textureCoveragePerSegment);
        }

        public void UpdateTexture()
        {
            Graphics.Blit(texture, buffer, material);
            Graphics.Blit(buffer, texture);
        }

        public float GetWetnessFromUVCoords(Vector2 uvCoords)
        {
            Texture2D texture2D = new Texture2D(
                texture.width,
                texture.height,
                TextureFormat.ARGB32,
                false,
                true
            );

            // copy the single pixel value from the render texture to the texture2D on the GPU

            RenderTexture.active = texture;
            texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;

            Color pixel = texture2D.GetPixel(
                Mathf.RoundToInt(uvCoords.x * texture2D.width),
                Mathf.RoundToInt(uvCoords.y * texture2D.height)
            );

            return pixel.r;
        }

    }
}
