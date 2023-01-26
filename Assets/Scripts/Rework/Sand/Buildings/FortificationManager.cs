namespace TideDefense
{
    using System;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using ToolBox.Pools;
    using System.Collections.Generic;

    public class FortificationManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GridManager _gridManager = null;

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private SandTower _prefabSandTower = null;

        private List<SandTower> _sandTowers = new List<SandTower>();

        [SerializeField]
        private Transform _fortificationContainer = null;

        [SerializeField]
        private SandCastle _sandCastle = null;

		#endregion

		#region Methods

        #region MonoBehaviour

        private void Start()
        {
            _sandCastle.Initialize(this);

            // Sand Castle
            _gridManager.SetSandCastleOnGrid(_sandCastle);

            InitializeFoundations();

            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool += CallbackOnChangeTool;
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool -= CallbackOnChangeTool;
        }

        #endregion

        public void BuildSandTower(GridCellModel gridCell, float sandWaterConcentration)
        {
            Vector3 worldPosition = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            // Build Sand Tower
            SandTower tower = UnityEngine.Object.Instantiate(
                _prefabSandTower,
                worldPosition,
                Quaternion.identity,
                _fortificationContainer
            );

            tower.Initialize(this, gridCell, sandWaterConcentration);
            _sandTowers.Add(tower);

            gridCell.building = tower;
        }

        public void DestroyBuilding(Building rempart)
        {
            Debug.Log($"Amount Sand Tower : {_sandTowers.Count}");
            if (rempart.GetType() == typeof(SandTower))
                _sandTowers.Remove((SandTower)rempart);
            Debug.Log($"Amount Sand Tower : {_sandTowers.Count}");

            rempart.gridCell.building = null;
            Destroy(rempart.gameObject);
        }

        public int GetRempartNeighboorsFromCoords(Vector2Int coords)
        {
            string bitmask = "";

            for (int i = 0; i < TilesetUtils.neighboorsCoordinatesFour.Count; i++)
            {
                Vector2Int offset = TilesetUtils.neighboorsCoordinatesFour[i];
                GridCellModel gridCell =
                    _gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                        new Vector2Int(coords.x + offset.x, coords.y + offset.y)
                    );

                // Tile tile = GridManager.Instance.CurrentGrid.GetTile(x + (int)offset.x, y + (int)offset.y);

                if (gridCell == null || gridCell.building == null)
                    bitmask += 0;
                else
                    bitmask += 1;
            }

            // Debug.Log(bitmask);
            int enumValue = Convert.ToInt32(bitmask, 2);
            // Debug.Log(enumValue);

            return enumValue;
        }

        #region Foundation



        [SerializeField]
        private RempartFoundation _foundationPrefab = null;
        public RempartFoundation foundationPrefab
        {
            get { return _foundationPrefab; }
        }

        private void InitializeFoundations()
        {
            _foundationPrefab.gameObject.Populate(12);
        }

        public void CallbackOnChangeTool(BeachTool beachTool)
        {
            if (beachTool.toolType == ToolType.Shovel)
                ActivateFoundationBuilders();
            else if (beachTool.toolType == ToolType.None)
                DeactivateFoundationBuilders();
        }

        public void ActivateFoundationBuilders()
        {
            foreach (SandTower tower in _sandTowers)
                tower.ActivateFoundationBuilder();
        }

        public void DeactivateFoundationBuilders()
        {
            foreach (SandTower tower in _sandTowers)
                tower.DeactivateFoundationBuilder();
        }

        #endregion

		#endregion
    }
}
