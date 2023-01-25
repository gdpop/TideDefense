namespace TideDefense
{
    using CodesmithWorkshop.Useful;
    using PierreMizzi.MouseInteractable;
    using UnityEngine;


    // TODO : 
    public class BuildRempartHandle : MonoBehaviour
    {
        
        private FortificationManager _fortificationManager = null;

        [SerializeField] private GridCellVisual _gridCell = null;
        public GridCellVisual gridCell { get { return _gridCell; } set { _gridCell = value; } }

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

        #endregion

        #region Foundation

        [SerializeField]
        private SandTower _sandTower = null;

        [SerializeField]
        private Transform _fondationContainer = null;

        private float _foundationTreshold = 0f;

        private float _cellSize = 0.25f;

        #endregion


        private void Start()
        {
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, transform.position);
            InitializeClickable();

            HideAllFoundation();

            _foundationTreshold = Mathf.Sqrt(Mathf.Pow(_cellSize / 2f, 2f) * 2f);
            Debug.Log(_foundationTreshold);
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
                int sideInt = Mathf.FloorToInt(rotation / 90f);
                _fondationContainer.transform.localRotation = Quaternion.Euler(
                    0f,
                    sideInt * -90f,
                    0f
                );

                // Number of foundation to display
                float length = _positionOnPlane.magnitude;
                length -= _foundationTreshold;
                length = Mathf.Max(0f, length);
                int amount = Mathf.FloorToInt(length / _cellSize);
                DisplayFoundation(amount);
            }
        }

        private void ReleaseHandle()
        {
            _isClicked = false;
            _isHandling = false;

            Debug.Log("Is Release !");
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
    }
}
