namespace TideDefense
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct MouldShape
    {
        public string name;

        [Range(0f, 359f)]
        public float rotation;

        public Fortification shape;
    }
}
