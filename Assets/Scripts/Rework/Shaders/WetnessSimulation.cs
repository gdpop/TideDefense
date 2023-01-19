namespace TideDefense
{
    using System.Collections;
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

        public CommandBuffer commandBuffer;

        [SerializeField]
        private float _updateFrequency = 0.5f;

        

        public IEnumerator UpdateTextureBehaviour()
        {
            while (true)
            {
                yield return new WaitForSeconds(_updateFrequency);
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

            WaveSimulation();

            StartCoroutine("UpdateTextureBehaviour");
        }

        public float _value = 0f;
        public const string VALUE_PROPERTY = "_Value";

        public void WaveSimulation()
        {
            DOVirtual
                .Float(
                    0f,
                    Mathf.PI,
                    5,
                    (float value) =>
                    {
                        _value = Mathf.Lerp(0f, 1f, Mathf.Sin(value));
                        material.SetFloat(VALUE_PROPERTY, _value);
                    }
                )
                .SetEase(Ease.InOutSine)
                .SetLoops(-1);
        }

        [ContextMenu("UpdateTexture")]
        public void UpdateTexture()
        {
            Graphics.Blit(texture, buffer, material);
            Graphics.Blit(buffer, texture);
        }
    }
}
