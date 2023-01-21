using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

namespace VirtuoseReality.Rendering
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialPropertyBlockModifier : MonoBehaviour
    {
        private static MaterialPropertyBlock s_materialPropertyBlock { get; set; }

        [Serializable]
        public class Property
        {
            public enum Type
            {
                Int,
                Float,
                Vector,
                Color,
                Texture
            }

            public string name;
            public Type type;
            public float floatValue;
            public int intValue;
            public Color32 colorValue;
            public Texture textureValue;
            public Vector4 vectorValue;
        }

        [SerializeField]
        private int m_materialIndex = 0;

        [SerializeField]
        private List<Property> m_propreties = new List<Property>();

        private new Renderer renderer { get; set; }

        private void Awake()
        {
            this.renderer = base.GetComponent<Renderer>();

            if (s_materialPropertyBlock == null)
                s_materialPropertyBlock = new MaterialPropertyBlock();

            this.ApplyProperties();
        }

        private void OnValidate()
        {
            if (s_materialPropertyBlock == null)
                s_materialPropertyBlock = new MaterialPropertyBlock();

            if (this.renderer == null)
                this.renderer = base.GetComponent<Renderer>();

            if (m_materialIndex >= this.renderer.sharedMaterials.Length)
                return;

            this.ApplyProperties();
        }

        private void ApplyProperties()
        {
            this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);

            foreach (Property property in m_propreties)
            {
                if (!this.renderer.sharedMaterials[m_materialIndex].HasProperty(property.name))
                    continue;

                switch (property.type)
                {
                    case Property.Type.Int:
                        s_materialPropertyBlock.SetInt(property.name, property.intValue);
                        break;

                    case Property.Type.Float:
                        s_materialPropertyBlock.SetFloat(property.name, property.floatValue);
                        break;

                    case Property.Type.Vector:
                        s_materialPropertyBlock.SetVector(property.name, property.vectorValue);
                        break;

                    case Property.Type.Color:
                        s_materialPropertyBlock.SetColor(property.name, property.colorValue);
                        break;

                    case Property.Type.Texture:
                        if (property.textureValue != null)
                            s_materialPropertyBlock.SetTexture(
                                property.name,
                                property.textureValue
                            );
                        else
                            s_materialPropertyBlock.SetTexture(
                                property.name,
                                Texture2D.whiteTexture
                            );
                        break;
                }
            }

            this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
        }

        public void SetFloat(string propertyName, float value)
        {
            Property property = m_propreties.Find(item => item.name == propertyName);

            property.floatValue = value;
            ApplyProperties();
        }
    }
}
