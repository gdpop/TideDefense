namespace TideDefense
{
    using System.Collections;
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
            // commandBuffer = new CommandBuffer();
            // commandBuffer.Blit(initialTexture, texture);
            buffer = new RenderTexture(
                texture.width,
                texture.height,
                texture.depth,
                texture.format
            );

            StartCoroutine("UpdateTextureBehaviour");
        }

        [ContextMenu("UpdateTexture")]
        public void UpdateTexture()
        {
            Graphics.Blit(texture, buffer, material);
            Graphics.Blit(buffer, texture);
            // commandBuffer.Blit(texture, buffer, material);
            // commandBuffer.Blit(buffer, texture);
        }
    }
}
