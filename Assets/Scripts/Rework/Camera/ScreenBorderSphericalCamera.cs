namespace TideDefense
{
    using UnityEngine;
    using CodesmithWorkshop;


	// TODO : Manage In and Out of Playmode for debugging
	// TODO : Manage initilization of the SphericalCameraCoordinate
	[ExecuteInEditMode]
    public class ScreenBorderSphericalCamera : SphericalCameraController
    {
        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

		#region Screen Border Control

        private Vector2 _screenDimensions = new Vector2();

        /// <summary>
        /// If mouse position is below the x value, camera rotates left
        /// If mouse position is above the y value, camera rotates right
        /// </summary>
        private Vector2 _minMaxVerticalTresholds = new Vector2();

        /// <summary>
        /// If mouse position is below the x value, camera moves down
        /// If mouse position is above the y value, camera moves up
        /// </summary>
        private Vector2 _minMaxHorizontalTresholds = new Vector2();

        [SerializeField]
        private float _tresholdPercentage = 0.25f;

        private Vector2 _centeredMousePosition = new Vector2();

        private bool isLeft = false;
        private bool isRight = false;

        private bool isDown = false;
        private bool isUp = false;

        private void Awake()
        {
            InitializeTresholds();
        }

        protected void Start()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onSetActiveSphericalCamera += CallbackSetActive;
        }

        protected override void Update()
        {
            if (!_isActive)
                return;

			if(UnityEngine.Application.isPlaying)
            	UpdateScreenBorderControl();

            base.Update();
        }

        protected void OnDestroy()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onSetActiveSphericalCamera -= CallbackSetActive;
        }

        private void InitializeTresholds()
        {
            _screenDimensions = new Vector2(Screen.width, Screen.height);
            // Debug.Log($"_screenDimensions : {_screenDimensions}");
            Vector2 screenTreshold = (_screenDimensions / 2f) * (1f - _tresholdPercentage);
            // Debug.Log($"screenTreshold : {screenTreshold}");
            _minMaxHorizontalTresholds = new Vector2(-screenTreshold.x, screenTreshold.x);
            _minMaxVerticalTresholds = new Vector2(-screenTreshold.y, screenTreshold.y);
        }

        private void UpdateScreenBorderControl()
        {
            _centeredMousePosition = new Vector2(
                (Input.mousePosition.x) - _screenDimensions.x / 2f,
                (Input.mousePosition.y) - _screenDimensions.y / 2f
            );

            isLeft = _centeredMousePosition.x < _minMaxHorizontalTresholds.x;
            isRight = _centeredMousePosition.x > _minMaxHorizontalTresholds.y;
            isDown = _centeredMousePosition.y < _minMaxVerticalTresholds.x;
            isUp = _centeredMousePosition.y > _minMaxVerticalTresholds.y;

            ManageHorizontalMotion(isLeft, isRight);
            ManageVerticalMotion(isUp, isDown);
        }

        #endregion

        private void CallbackSetActive(bool isActive)
        {
            _isActive = isActive;
        }
    }
}
