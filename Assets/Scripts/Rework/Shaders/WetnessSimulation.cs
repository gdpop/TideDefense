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

        [SerializeField] private float _evaporationSpeed = 0.1f;

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

            // WaveSimulation();

            StartCoroutine("UpdateTextureBehaviour");
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
    }
}
