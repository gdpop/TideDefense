namespace TideDefense
{
    using UnityEngine;

    public class WaveSegment : MonoBehaviour
    {
#region Fields

        [SerializeField]
        private transform _visualTransform = null;

        private Vector3 _visualLocalScale = null;

#endregion

#region Methods

		public void Start()
		{
			_visualLocalScale = _visualTransform.localScale;
		}


        public void CrashOnBeach(float delay, float strength)
        {
            DOVirtual.Float(
                0f,
                1f,
                5f,
                (float value) =>
                {
_visualLocalScale = 

                    _visualTransform.localScale = new Vector3(1);
                }
            );
        }
#endregion
    }
}
