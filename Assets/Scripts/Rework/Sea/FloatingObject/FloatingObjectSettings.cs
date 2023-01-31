namespace TideDefense
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "FloatingObjectSettings",
        menuName = "TideDefense/FloatingObjectSettings",
        order = 0
    )]
    public class FloatingObjectSettings : ScriptableObject
    {
        [Header("Spawning Zone")]
        public Vector2 floatingSpawnZoneDimensions = new Vector2();
        public float submergedOffsetY = 1f;
        public Vector3 offsetFloatingContainer = new Vector3();

        [Header("Movement")]
        [Header("Apparition")]
        public float apparitionDuration = 1f;

        [Header("Forward")]
        public float forwardSpeed = 0.1f;

        [Header("Up and Down")]
        public float upAndDownSpeed = 1f;
        public float _upAndDownAmplitude = 1f;
    }
}
