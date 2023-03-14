using System.Collections.Generic;

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
        private List<Color> _colorFromQuality = new List<Color>();
        public List<Color> colorFromQuality
        {
            get { return _colorFromQuality; }
        }

        [SerializeField]
        private float _qualityCoef = 100f;
        public float qualityCoef
        {
            get { return _qualityCoef; }
        }

        public Color GetColorFromQuality(int quality)
        {
            if (0 < quality && quality < _colorFromQuality.Count)
            {
                return _colorFromQuality[quality];
            }
            else
                return Color.black;
        }

        #region Wave Dame

        [SerializeField]
        private AnimationCurve _damageDealtByWave;
        public AnimationCurve damageDealtByWave
        {
            get { return _damageDealtByWave; }
        }

        #endregion

		#endregion
    }
}
