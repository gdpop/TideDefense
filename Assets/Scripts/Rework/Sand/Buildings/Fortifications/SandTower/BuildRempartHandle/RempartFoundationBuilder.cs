namespace TideDefense
{
    using System.Collections.Generic;
    using CodesmithWorkshop.Useful;
    using PierreMizzi.MouseInteractable;
    using PierreMizzi.TilesetUtils;
    using ToolBox.Pools;
    using UnityEngine;

    /// <summary>
    /// Drag & drop rempart foundation on the beach, originated from a Sand Tower
    /// Floats above a Sand Tower when the shovel is grabbed
    /// </summary>
    public class RempartFoundationBuilder : MonoBehaviour
    {
        private FortificationManager _manager
        {
            get { return _sandTower.fortificationManager; }
        }

        private float _cellSize = 0f;

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
        private int _selectedSide = 0;

        /// <summary>
        /// Amount of rempartFoundation layed by the player
        /// </summary>
        private int _selectedFoundationAmount = 0;

        #endregion

        #region Foundation

        [SerializeField]
        private Transform _fondationContainer = null;

        private float _foundationTreshold = 0f;

        private List<RempartFoundation> _foundations = new List<RempartFoundation>();

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, transform.position);
            InitializeClickable();
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

        public void Initialize(SandTower tower)
        {
            _sandTower = tower;
            _cellSize = _manager.gridManager.cellSize;
            _foundationTreshold = Mathf.Sqrt(Mathf.Pow(_cellSize / 2f, 2f) * 2f);
        }

        public void Activate()
        {
            _visual.SetActive(true);
            _clickable.isInteractable = true;
        }

        public void Deactivate()
        {
            _visual.SetActive(false);
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

                leftClick.onMouseDown += CallbackStartHandle;
            }
        }

        private void CallbackStartHandle(RaycastHit hit)
        {
            _isClicked = true;
            ManageFoundationsInitialization();
        }

        private void UpdateHandle()
        {
            _screenRay = _camera.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(_screenRay, out _enter))
            {
                _positionOnPlane = _screenRay.GetPoint(_enter) - transform.position;
                _positionOnPlane.y = 0;
                _debugTransform.localPosition = _positionOnPlane;

                // Rotation around the tower
                float rotation =
                    Mathf.Rad2Deg * Mathf.Atan2(_positionOnPlane.z, _positionOnPlane.x);
                rotation = UtilsClass.ToFullAngle(rotation);
                rotation = UtilsClass.OffsetFullAngle(rotation, 45f);
                _selectedSide = Mathf.FloorToInt(rotation / 90f);
                _fondationContainer.transform.localRotation = Quaternion.Euler(
                    0f,
                    _selectedSide * -90f,
                    0f
                );

                // Number of foundation to display
                float length = _positionOnPlane.magnitude;
                length -= _foundationTreshold;
                length = Mathf.Max(0f, length);
                _selectedFoundationAmount = Mathf.FloorToInt(length / _cellSize);

                ManageFoundationsHandling(_selectedFoundationAmount);
            }
        }

        private void ReleaseHandle()
        {
            _isClicked = false;
            _isHandling = false;

            Debug.Log($"Side : {_selectedSide} | Amount : {_selectedFoundationAmount}");
            ManageFoundationsReleasing();
            // _manager.ReleaseHandleFoundationBuilder();

            _selectedSide = -1;
            _selectedFoundationAmount = 0;
        }

        #endregion

        #region Foundation


        /// <summary>
        /// Pools foundations from FortificationManager and position them in their container
        /// </summary>
        private void ManageFoundationsInitialization()
        {
            RempartFoundation foundation;

            for (int i = 0; i < _manager.handledFoundationPrefabAmount; i++)
            {
                foundation = _manager.foundationPrefab.Reuse<RempartFoundation>();
                foundation.transform.SetParent(_fondationContainer);
                foundation.transform.localPosition = new Vector3((i + 1) * _cellSize, 0f, 0f);

                foundation.gameObject.SetActive(false);
                _foundations.Add(foundation);
            }
        }

        /// <summary>
        /// Either build or release foundations according to selected amount by the player
        /// </summary>
        private void ManageFoundationsReleasing()
        {
            RempartFoundation foundation;
            Vector2Int coords;

            int count = _foundations.Count;
            for (int i = 0; i < count; i++)
            {
                foundation = _foundations[i];
                // First, we build the selected amount of Foundations
                if (i + 1 <= _selectedFoundationAmount)
                {
                    coords = GetHandledCoords(i);
                    _manager.BuildFoundation(_sandTower, foundation, coords);
                }
                else
                {
                    foundation.gameObject.Release();
                }
            }
            _foundations.Clear();
        }

        private void ManageFoundationsHandling(int amount)
        {
            Vector2Int coords;
            bool validCoords;
            GridCellModel cellModel;
            RempartFoundation foundation;

            float count = _foundations.Count;
            for (int i = 0; i < count; i++)
            {
                coords = GetHandledCoords(i + 1);
                foundation = _foundations[i];

                validCoords = _manager.gridManager.gridModel.CheckValidCoordinates(coords);
                // If the coords aren't valid, it's useless to check even further. It's always out of bound
                if (!validCoords)
                    break;

                // _sandTower.ManageLinkingRemparts(_selectedSide);

                cellModel = _manager.gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                    coords
                );

                if ((i <= amount - 1) && validCoords && cellModel.isEmpty)
                {
                    foundation.gameObject.SetActive(true);
                    foundation.transform.localPosition = _fondationContainer.InverseTransformPoint(
                        _manager.gridManager.gridModel.GetCellWorldPositionFromCoordinates(coords)
                    );
                }
                else
                    foundation.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Depending on the _selectedSide and the amount given, returns the corresponding grid coordinates
        /// </summary>
        private Vector2Int GetHandledCoords(int amount)
        {
            return _sandTower.gridCellModel.coords
                + (TilesetUtils.trigNeighboorsCoordinatesFour[_selectedSide] * amount);
        }

        #endregion
    }
}
