namespace TideDefense
{
    using System.Collections.Generic;
    using DG.Tweening;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using VirtuoseReality.Extension.AudioManager;
    using VirtuoseReality.Helpers;

    public class GameplayManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;
        public GameplayChannel gameplayChannel
        {
            get { return _gameplayChannel; }
        }

        [SerializeField]
        private UIChannel _UIChannel = null;
        public UIChannel UIChannel
        {
            get { return _UIChannel; }
            set { _UIChannel = value; }
        }

        [SerializeField]
        private FortificationManager _fortificationManager = null;
        public FortificationManager fortificationManager
        {
            get { return _fortificationManager; }
        }

        [SerializeField]
        private SeaManager _seaManager = null;
        public SeaManager seaManager
        {
            get { return _seaManager; }
        }

        [SerializeField]
        private GridManager _gridManager = null;
        public GridManager gridManager
        {
            get { return _gridManager; }
        }

        #region State Behaviour

        private BaseGameplayBehaviour _currentStateBehaviour = null;

        private Dictionary<BeachToolType, BaseGameplayBehaviour> _stateBehaviours =
            new Dictionary<BeachToolType, BaseGameplayBehaviour>();

        #endregion

        #region Beach Tool

        [SerializeField]
        private List<BeachTool> _availableTools = new List<BeachTool>();

        /// <summary>
        /// Tool that does nothing. onChangeTool.Invoke(null); does nothing so I use it here
        /// </summary>
        private BeachTool _noneTool = new BeachTool();

        [Header("Tool")]
        [SerializeField]
        private BeachTool _currentTool = null;

        public BeachTool currentTool
        {
            get { return _currentTool; }
        }

        [SerializeField]
        private float _shovelFillingQuantity = 0.25f;
        public float shovelFillingQuantity
        {
            get { return _shovelFillingQuantity; }
            set { _shovelFillingQuantity = value; }
        }

        #endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickTool += CallbackOnClickTool;
            }

            // Initialize the bucket as dropped on the beach
            InitializeStateBehaviour();

            foreach (BeachTool beachTool in _availableTools)
                InitializeTool(beachTool);
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickTool -= CallbackOnClickTool;
            }
        }

		#endregion

        #region State Behaviour

        private void InitializeStateBehaviour()
        {
            _stateBehaviours = new Dictionary<BeachToolType, BaseGameplayBehaviour>()
            {
                { BeachToolType.None, new GameplayBehaviourIdle(this) },
                { BeachToolType.Shovel, new GameplayBehaviourShovel(this) },
                { BeachToolType.Mould, new GameplayBehaviourMould(this) },
            };

            ChangeStateBehaviour(BeachToolType.None);
        }

        private void ChangeStateBehaviour(BeachToolType toolType)
        {
            foreach (KeyValuePair<BeachToolType, BaseGameplayBehaviour> pair in _stateBehaviours)
            {
                if (BitMaskHelper.CheckMask((int)toolType, (int)pair.Key))
                {
                    _currentStateBehaviour = pair.Value;
                    _currentStateBehaviour.Activate();
                }
            }
        }

        #endregion

        #region Grid Manager

        public void DropToolOnGrid(BeachTool tool, GridCellModel gridCell)
        {
            _gridManager.DropToolOnGrid(tool, gridCell);
        }

        public void PickToolOnGrid(BeachTool tool, GridCellModel gridCell)
        {
            _gridManager.PickToolOnGrid(gridCell);
        }

        #endregion

        #region Beach Tool

        private void InitializeTool(BeachTool tool)
        {
            tool.Initialize(this);
            GridCellModel gridCell = _gridManager.gridModel.GetCellFromWorldPosition<GridCellModel>(
                tool.transform.position
            );

            if (gridCell != null)
            {
                _gridManager.DropToolOnGrid(tool, gridCell);
                tool.SetDropped(gridCell);
            }
        }

        public void CallbackOnClickTool(BeachTool tool)
        {
            if (_currentTool == null)
                GrabTool(tool);
        }

        public void SetCurrentTool(BeachTool tool)
        {
            _currentStateBehaviour.Deactivate();
            _currentTool = tool;
            ChangeStateBehaviour(_currentTool.toolType);
            _gameplayChannel.onChangeTool.Invoke(_currentTool);
        }

        [SerializeField]
        private BeachToolHolder _toolHolder = null;


        public void GrabTool(BeachTool tool)
        {
            SetCurrentTool(tool);

            _toolHolder.GrabTool(
                tool,
                () =>
                {
                    _currentTool.SetGrabbed();
                }
            );

            if(tool.isWashedUp)
            {
                _availableTools.Add(tool);
                tool.isWashedUp = false;
            }else
                PickToolOnGrid(tool, tool.currentGridCell);
        }

        public void DropTool(GridCellModel gridCell)
        {
            Vector3 to = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            _toolHolder.DropTool(
                to,
                () =>
                {
                    DropToolOnGrid(_currentTool, gridCell);
                    _currentTool.SetDropped(gridCell);
                    SetCurrentTool(_noneTool);
                }
            );
        }

        #region Display Diggable

        public void DisplayDiggableHints()
        {
            // List of coordinates where there is multiple BeachToolType.Container around
            List<Vector2Int> duplicateDiggableCoords = new List<Vector2Int>();
            // Key = Coords in the grid, Value = Offset to find the tool next to hit
            Dictionary<Vector2Int, Vector2Int> coordWithOffsetCoords =
                new Dictionary<Vector2Int, Vector2Int>();

            foreach (BeachTool tool in _availableTools)
            {
                if (BitMaskHelper.CheckMask((int)tool.toolType, (int)BeachToolType.Mould))
                {
                    Vector2Int tilesetCoords = new Vector2Int();
                    Vector2Int neighboorCoords = new Vector2Int();

                    // We go through all 8 surrouding cells
                    for (int i = 0; i < TilesetUtils.neighboorsCoordinatesEight.Count; i++)
                    {
                        tilesetCoords = TilesetUtils.neighboorsCoordinatesEight[i];
                        neighboorCoords = tool.currentGridCell.coords + tilesetCoords;

                        if (
                            !duplicateDiggableCoords.Contains(neighboorCoords)
                            && !coordWithOffsetCoords.ContainsKey(neighboorCoords)
                        )
                        {
                            coordWithOffsetCoords.Add(neighboorCoords, -tilesetCoords);
                        }
                        else
                        {
                            // Debug.Log($"The cell is shared ! : {neighboorCoords}");
                            if (duplicateDiggableCoords.Contains(neighboorCoords))
                                duplicateDiggableCoords.Add(neighboorCoords);

                            if (coordWithOffsetCoords.ContainsKey(neighboorCoords))
                                coordWithOffsetCoords.Remove(neighboorCoords);
                        }
                    }
                }
            }

            _gridManager.DisplayDiggableHints(coordWithOffsetCoords);
        }

        public void HideDiggableHints() { }

        #endregion

        #endregion

    	#endregion
    }
}
