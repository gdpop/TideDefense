namespace TideDefense
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "FortificationChannel",
        menuName = "TideDefense/FortificationChannel",
        order = 0
    )]
    public class FortificationChannel : ScriptableObject
    {
		#region Quality and Sand Concentration

        [SerializeField]
        private AnimationCurve _qualityFromSandConcentration = null;
        public AnimationCurve qualityFromSandConcentration
        {
            get { return _qualityFromSandConcentration; }
        }

        [SerializeField]
        private float _qualityCoef = 100f;
        public float qualityCoef
        {
            get { return _qualityCoef; }
        }

		#endregion
    }
}
