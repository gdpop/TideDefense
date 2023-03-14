namespace TideDefense
{
    using PierreMizzi.MouseInteractable;
    using System.Collections;
    using UnityEngine;

    public class BottleTranslator : Building
    {
		#region Fields

        [SerializeField]
        private MessageBottleSettings _messageBottleSettings = null;

        [SerializeField]
        private TimeChannel _timeChannel = null;

        [SerializeField]
        private BottleTranslatorState _state = BottleTranslatorState.None;

        private MessageBottle _messageBottle = null;

        [SerializeField]
        private HoldClickable _clickable = null;

        #region Translation Behaviour

        private IEnumerator _translateBottleCoroutine = null;
        private float _translationTime = 0f;

        #endregion

        #region Stalling Behaviour

        private IEnumerator _stallingCoroutine = null;
        private float _stallingTime = 0f;

        #endregion



		#endregion

		#region Methods

        #region MonoBehaviour

        private void Start()
        {
            _state = BottleTranslatorState.Available;
            if (_timeChannel != null)
                _timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
        }

        private void OnDestroy()
        {
            if (_timeChannel != null)
                _timeChannel.onUpdateCurrentDeltaTime -= CallbackUpdateCurrentDeltaTime;
        }

        #endregion

        private void ChangeState(BottleTranslatorState state)
        {
            switch (state)
            {
                case BottleTranslatorState.Available:
                    SetStateAvailable();
                    break;

                case BottleTranslatorState.Translating:
                    SetTranslatingState();
                    break;

                case BottleTranslatorState.Stalling:
                    SetStateStalling();
                    break;
            }
        }

        public void AssignMessageBottle(MessageBottle messageBottle)
        {
            _messageBottle = messageBottle;
            ChangeState(BottleTranslatorState.Translating);
        }

        public override void ManageWaveCollision(float waveStrength)
        {
            _stallingTime += _messageBottleSettings.penaltyDurationDealtByWave.Evaluate(
                waveStrength
            );

            SetStateStalling();
        }

        private void CallbackUpdateCurrentDeltaTime(float currentDeltaTime)
        {
            if (_state == BottleTranslatorState.Translating)
            {
                _translationTime -= currentDeltaTime;
                UdpateTranslatingState();
            }
            else if (_state == BottleTranslatorState.Stalling)
            {
                _stallingTime -= currentDeltaTime;
                UdpateStallingState();
            }
        }

        #region State - Available

        public void SetStateAvailable()
        {
            _state = BottleTranslatorState.Available;
        }

        #endregion

        #region State - Translating

        public void SetTranslatingState()
        {
            switch (_state)
            {
                // Initialize Translation
                case BottleTranslatorState.Available:
                    _translationTime = _messageBottleSettings.translationDuration;
                    break;

                // Resume Translating
                case BottleTranslatorState.Stalling:
                    break;
            }

            _state = BottleTranslatorState.Translating;
        }

        private void UdpateTranslatingState()
        {
            if (_translationTime <= 0)
            {
                Debug.Log("TRANSLATION COMPLETE");
                ChangeState(BottleTranslatorState.Available);
            }
        }

        #endregion

        #region State - Stalling

        public void SetStateStalling()
        {
            switch (_state)
            {
                // Initialize Translation
                case BottleTranslatorState.Translating:
                    _state = BottleTranslatorState.Stalling;

                    break;
                // Resume Translating
                case BottleTranslatorState.Stalling:
                    _state = BottleTranslatorState.Stalling;
                    break;
            }
        }

        private void UdpateStallingState()
        {
            if (_stallingTime <= 0)
            {
                Debug.Log("STALLING COMPLETE, RESUME TRANSLATION");
                _stallingTime = 0;
                ChangeState(BottleTranslatorState.Translating);
            }
        }

        #endregion

        [ContextMenu("AssignMessageBottle")]
        public void AssignMessageBottle()
        {
            ChangeState(BottleTranslatorState.Translating);
        }

        [ContextMenu("Debug Stalling")]
        public void DebugStalling()
        {
            ManageWaveCollision(0.5f);
        }

		#endregion
    }
}
