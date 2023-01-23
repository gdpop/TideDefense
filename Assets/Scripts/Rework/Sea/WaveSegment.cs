namespace TideDefense
{
    using UnityEngine;
    using DG.Tweening;
    using PierreMizzi.Collisions;
    using System.Collections;

    public class WaveSegment : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private SeaChannel _seaChannel = null;

        private Wave _wave;

        [SerializeField]
        private Transform _visualTransform = null;

        private Vector3 _visualLocalScale;

        [SerializeField]
        private TriggerInteractable _visualLocalInteractable = null;

        /// <summary>
        /// The unique number given to this segment
        /// </summary>
        private int _segmentIndex = 0;


        private float _beachCoverage = 0f;

        /// <summary> 
        /// Similar to SeaManager.tideLevel. Represent how much he covers the beach with it's length
        /// </summary>
        public float beachCoverage { get { return _beachCoverage; } }

        private Tween _crashingTween = null;

        /// <summary>
        /// Called when this segment returned to the sea
        /// </summary>
        public WaveSegmentDelegate onDisappear = null;

        private Building _collidedBuilding = null;

        #endregion

        #region Methods

        #region MonoBehaviour

        private void CallbackOnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Building>(out _collidedBuilding))
            {
                CrashOnBuilding(_collidedBuilding);
            }
            else if (_collidedBuilding != null)
                _collidedBuilding = null;
        }

        #endregion

        public void CrashOnBeach(Wave wave, int firstSegmentIndex, float delay, int strongestSegmentIndex, float strength)
        {
            _wave = wave;

            _visualLocalScale = _visualTransform.localScale;
            _visualLocalInteractable.Initialize();
            _visualLocalInteractable.onTriggerEnter += CallbackOnTriggerEnter;
            _segmentIndex = transform.GetSiblingIndex();

            // TODO : Switch from linear function to curved function
            // How much delay should be applied based on firstSegmentIndex.The further the longer. Creates this "curve" effect of the wave
            float totalDelay = Mathf.Abs(_segmentIndex - firstSegmentIndex) * delay;

            // Manage the individual strength of each segment for more chaotic looking wave
            float individualStrength = Mathf.Abs(_segmentIndex - strongestSegmentIndex) / (float)_wave.amountWaveSegment;
            individualStrength = _seaChannel.strengthIndividualWaveSegment.Evaluate(individualStrength); 

            _crashingTween = DOVirtual
                .Float(
                    0f,
                    Mathf.PI,
                    _seaChannel.waveCrashDuration,
                    (float value) =>
                    {
                        _beachCoverage = Mathf.Lerp(0f, strength + individualStrength, Mathf.Sin(value));
                        _visualLocalScale.z = beachCoverage;
                        _visualTransform.localScale = _visualLocalScale;
                    }
                )
                .SetEase(_seaChannel.waveCrashEase)
                .SetDelay(totalDelay)
                .OnComplete(ReturnedToSea);
        }

        private void ReturnedToSea()
        {
            onDisappear.Invoke(this);
            _visualLocalInteractable.onTriggerEnter -= CallbackOnTriggerEnter;
        }

        /// <summary>
        /// Manages segments tween behaviour and damages dealt to the rempart
        /// </summary>
        private void CrashOnBuilding(Building collidedBuilding)
        {
            // Pauses the tween
            _crashingTween.Pause();

            float elapsedPercentage = (float)_crashingTween.ElapsedPercentage();

            // Determines how long the waves is pause before going back to the sea. The sooner the collision, the longer the pause.
            // Note :
            // Tween value from 0 to 0.5 : Segment is crashing, moving forward
            // Tween value from 0.5 to 1 : Segment is retiring, moving backward

            // Here we try to find the "mirrored" value when wave should be retiring
            // For exemple : Crashes at 0.25, retires at 0.75
            float pauseDuration =
                ((0.5f - (elapsedPercentage % 0.5f)) * 2f) * (float)_crashingTween.Duration();

            // Debug.Log($"mirrorElapsed : {elapsedPercentage} | pauseDuration : {pauseDuration}");

            // Play the tween backward after the right delay
            DOVirtual.DelayedCall(
                pauseDuration,
                () =>
                {
                    _crashingTween.PlayBackwards();
                    StartCoroutine("WaitForRewind");
                }
            );

            // Inflict damage to the rempart
            ManageDamagedRempart(collidedBuilding, elapsedPercentage);
        }

        private IEnumerator WaitForRewind()
        {
            yield return _crashingTween.WaitForRewind();
            ReturnedToSea();
        }

        private void ManageDamagedRempart(Building building, float elapsedPercentage)
        {
            float normalizedElapsedPercentage = elapsedPercentage * 2f;
            float amountDamageDealt = _seaChannel.damageDealtByWave.Evaluate(normalizedElapsedPercentage);

            Debug.Log($"normalized : {normalizedElapsedPercentage} | damageDealt : {amountDamageDealt}");

            building.InflictDamage(amountDamageDealt);
        }

        #endregion


    }
}
