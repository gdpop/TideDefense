namespace TideDefense
{
    using CodesmithWorkshop.Useful;
    using PierreMizzi.MouseInteractable;
    using ToolBox.Pools;
    using UnityEngine;

    /// <summary>
    /// Drag & drop rempart foundation on the beach, originated from a Sand Tower
    /// Floats above a Sand Tower when the shovel is grabbed
    /// </summary>
    public class RempartFoundationBuilder : MonoBehaviour
    {
        private FortificationManager _fortificationManager = null;

        [SerializeField]
        private GridCellModel _gridCell = null;
        public GridCellModel gridCell
        {
            get { return _gridCell; }
            set { _gridCell = value; }
        }

        public SandTower _sandTower = null;

        [SerializeField]
        public GameObject _visual = null;

        #region Raycast in plane

        private Plane _plane;
        private Camera _camera;
        private Ray _screenRay;
        private float _enter;
        private Vector3 _positionOnPlane;

        #endregion

        #region Handle Clickable

        [SerializeField]
        private Transform _debugTransform = null;

        [SerializeField]
        private HoldClickable _clickable = null;
        private bool _isClicked = false;
        private bool _isHandling = false;

        /// <summary>
        /// Current side targeted by the player when building foundation
        /// 0 = Right, 1 = forward, 2 = Left, 3 = backward
        /// </summary>
        private int _handledSide = 0;

        /// <summary>
        /// Amount of rempartFoundation layed by the player while targeting
        /// </summary>
        private int _handledAmountFoundation = 0;

        #endregion

        #region Foundation

        [SerializeField]
        private Transform _fondationContainer = null;

        private float _foundationTreshold = 0f;

        private float _cellSize = 0.25f;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, transform.position);
            InitializeClickable();

            HideAllFoundation();

            _foundationTreshold = Mathf.Sqrt(Mathf.Pow(_cellSize / 2f, 2f) * 2f);
        }

        private void Update()
        {
            if (_isClicked)
            {
                _isHandling = Input.GetMouseButton(InteractableManager.MOUSE_LEFT);

                if (_isHandling)
                    UpdateHandle();
                else
                    ReleaseHandle();
            }
        }

        #endregion

        public void Activate()
        {
            _visual.SetActive(true);
            _clickable.isInteractable = true;
        }

        public void Deactivate()
        {
            _visual.SetActive(true);
            _clickable.isInteractable = true;
        }

        #region Handle Clickable

        private void InitializeClickable()
        {
            if (_clickable != null)
            {
                HoldClickBehaviour leftClick = _clickable.GetBehaviour(
                    InteractableManager.MOUSE_LEFT
                );

                leftClick.onMouseDown += CallbackMouseDown;
            }
        }

        private void CallbackMouseDown(RaycastHit hit)
        {
            _isClicked = true;
            Debug.Log("CallbackMouseDown");
        }

        private void UpdateHandle()
        {
            _screenRay = _camera.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(_screenRay, out _enter))
            {
                _positionOnPlane = _screenRay.GetPoint(_enter);
                _positionOnPlane.y = 0;
                _debugTransform.localPosition = _positionOnPlane;

                // Rotation around the tower
                float rotation =
                    Mathf.Rad2Deg * Mathf.Atan2(_positionOnPlane.z, _positionOnPlane.x);
                rotation = UtilsClass.ToFullAngle(rotation);
                rotation = UtilsClass.OffsetFullAngle(rotation, 45f);
                _handledSide = Mathf.FloorToInt(rotation / 90f);
                _fondationContainer.transform.localRotation = Quaternion.Euler(
                    0f,
                    _handledSide * -90f,
                    0f
                );

                // Number of foundation to display
                float length = _positionOnPlane.magnitude;
                length -= _foundationTreshold;
                length = Mathf.Max(0f, length);
                _handledAmountFoundation = Mathf.FloorToInt(length / _cellSize);
                DisplayFoundation(_handledAmountFoundation);
            }
        }

        private void ReleaseHandle()
        {
            _isClicked = false;
            _isHandling = false;

            Debug.Log($"Side : {_handledSide} | Amount : {_handledAmountFoundation}");

            _handledSide = -1; 
            _handledAmountFoundation = 0;
        }

        #endregion

        #region Foundation

        public void Initialize(SandTower tower)
        {
            _sandTower = tower;
            // _sandTower.fortificationManager.foundationPrefab.gameObject.Reuse
        }

        private void HideAllFoundation()
        {
            foreach (Transform child in _fondationContainer)
                child.gameObject.SetActive(false);
        }

        private void DisplayFoundation(int amount)
        {
            foreach (Transform child in _fondationContainer)
            {
                child.gameObject.SetActive(child.GetSiblingIndex() <= amount - 1);
            }
        }

        #endregion
    }
}
