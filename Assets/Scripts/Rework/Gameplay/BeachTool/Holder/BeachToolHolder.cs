namespace TideDefense
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;
    using VirtuoseReality.Extension.AudioManager;

    public class BeachToolHolder : MonoBehaviour
    {
		#region Fields

        #region Behaviour

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private Transform _gameplayContainer = null;

        private BeachTool _currentTool = null;

        [SerializeField]
        private Vector3 _groundOffset = new Vector3();

        [SerializeField]
        private float _tweenDuration = 0.5f;
        #endregion

		#region Physics

        [Header("Physics")]
        [SerializeField]
        private Rigidbody _connectedBody = null;

        [SerializeField]
        private Rigidbody _joint = null;

        [SerializeField]
        private Transform _anchor = null;

		#endregion

        #region Manage Rotation

        private bool _updateRotation = false;

        public bool updateRotation
        {
            get { return _updateRotation; }
            set { _updateRotation = value; }
        }

        // Scrolling Settings
        private float _scrollingValue = 0f;

        [Header("Scrolling Rotation")]
        [SerializeField]
        private float _scrollingSpeed = 0.1f;

        // Rotation Settings
        private int _currentStep = 0;
        private int _lastStep = 0;

        [SerializeField]
        private float _rotationSpeed = 0.5f;

        private List<float> _allowedRotation = new List<float>();

        private int _allowedRotationIndex = 0;

        #endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            if (_gameplayChannel.onHoverBeach != null)
                _gameplayChannel.onHoverBeach += CallbackOnHoverBeach;
        }

        protected virtual void Update()
        {
            if (_updateRotation)
                ManageRotation();
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

            InitializeRotation();

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

            StopRotation();

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

        #region Manage rotation

        private void InitializeRotation()
        {
            RempartMould mould;
            if (_currentTool.TryGetComponent(out mould))
            {
                _updateRotation = true;

                foreach (MouldShape shape in mould.mouldedShapes)
                {
                    _allowedRotation.Add(shape.rotation);
                }
            }
            else
            {
                _updateRotation = false;
            }
        }

        private void StopRotation()
        {
            _updateRotation = false;
            _allowedRotation.Clear();
            _scrollingValue = 0;
            _currentStep = 0;
            _lastStep = 0;
            _allowedRotationIndex = 0;
        }

        private void ManageRotation()
        {
            _scrollingValue += Input.mouseScrollDelta.y * _scrollingSpeed;

            _currentStep = Mathf.FloorToInt(_scrollingValue / 2f);

            if (_currentStep != _lastStep)
            {
                _lastStep = _currentStep;
                Rotate();
            }
        }

        private void Rotate()
        {
            _allowedRotationIndex = Mathf.FloorToInt(
                Mathf.Abs(_currentStep) % _allowedRotation.Count
            );
            Vector3 eulerAngles = new Vector3(0f, _allowedRotation[_allowedRotationIndex], 0f);
            _currentTool.transform.DOLocalRotate(eulerAngles, _rotationSpeed).SetEase(Ease.InQuad);
            ((RempartMould)_currentTool).mouldShapeIndex = _allowedRotationIndex;
        }

        #endregion

		#endregion
    }
}
