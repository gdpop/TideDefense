namespace TideDefense
{
    using System;
    using DG.Tweening;
    using UnityEngine;
    using VirtuoseReality.Extension.AudioManager;

    public class BeachToolHolder : MonoBehaviour
    {
		#region Fields
        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private Transform _gameplayContainer = null;

        private BeachTool _currentTool = null;

		[SerializeField] private Vector3 _groundOffset = new Vector3();

        [SerializeField]
        private float _tweenDuration = 0.5f;

		#region Physics

        [Header("Physics")]
        [SerializeField]
        private Rigidbody _connectedBody = null;

        [SerializeField]
        private Rigidbody _joint = null;

        [SerializeField]
        private Transform _anchor = null;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            if (_gameplayChannel.onHoverBeach != null)
                _gameplayChannel.onHoverBeach += CallbackOnHoverBeach;
        }

        private void OnDestroy()
        {
            if (_gameplayChannel.onHoverBeach != null)
                _gameplayChannel.onHoverBeach -= CallbackOnHoverBeach;
        }

		#endregion

		#region Behaviour

        public void CallbackOnHoverBeach(RaycastHit hit)
        {
            _connectedBody.MovePosition(hit.point + _groundOffset);
        }

        public void GrabTool(BeachTool tool, Action onComplete)
        {
            _currentTool = tool;
            // Tween to lerp the tool from the ground to the mouse
            Vector3 from = _currentTool.transform.position;
            LockBucketJoint();

            DOVirtual
                .Float(
                    0f,
                    1f,
                    _tweenDuration,
                    (float value) =>
                    {
                        _joint.transform.position = _connectedBody.position;
                        _currentTool.transform.position = Vector3.Lerp(
                            from,
                            _anchor.position,
                            value
                        );
                    }
                )
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    // Let the tool hang free
                    FreeBucketJoint();
                    _currentTool.transform.SetParent(_joint.transform);
                    _currentTool.transform.localPosition = _anchor.localPosition;
                });

            SoundManager.PlaySound(SoundDataIDStatic.BEACH_TOOL_GRAB);
        }

        public void DropTool(Vector3 position, TweenCallback onComplete)
        {
            LockBucketJoint();
            _currentTool.transform.SetParent(_gameplayContainer);

            _currentTool.transform
                .DOMove(position, _tweenDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(onComplete);

            SoundManager.PlaySound(SoundDataIDStatic.BEACH_TOOL_DROP);
        }

		#endregion

		#region Physics

        private void LockBucketJoint()
        {
            _joint.isKinematic = true;
            _joint.useGravity = false;
            _joint.angularDrag = 0;
            _joint.angularVelocity = Vector3.zero;
            _joint.transform.position = _connectedBody.position;
            _joint.transform.localRotation = Quaternion.identity;
        }

        private void FreeBucketJoint()
        {
            _joint.isKinematic = false;
            _joint.useGravity = true;
            _joint.angularDrag = 0;
            _joint.angularVelocity = Vector3.zero;
            _joint.transform.position = _connectedBody.position;
            _joint.transform.localRotation = Quaternion.identity;
        }

		#endregion


		#endregion
    }
}
