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
        public float upAndDownAmplitude = 0.01f;


        [Header("WashUp")]
        public Vector3 washUpOffset = new Vector3();

        public float apexBeachCoveragePercent = 0.75f;

        public float washUpDuration = 3f; 

        public AnimationCurve washUpEase = null;
    }
}
