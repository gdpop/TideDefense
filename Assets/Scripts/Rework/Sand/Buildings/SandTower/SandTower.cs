namespace TideDefense
{
    using System.Collections.Generic;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    // TODO : Dissociate Normalized Health & Actual Health
    public class SandTower : Building
    {
        #region Fields

        [SerializeField]
        private MaterialPropertyBlockModifier _materialPropertyBlock = null;

        [SerializeField]
        private RempartFoundationBuilder _foundationBuilder = null;

        #region Flag Pole

        [SerializeField]
        private FlagPole _flagPole = null;

        #endregion

        #region Rempart Foundation Builder

        private List<Building> _neighboorBuildings = new List<Building>();

        [SerializeField]
        private List<GameObject> _linkingFoundations = new List<GameObject>();

        #endregion

        #endregion

        #region Methods

        private void Awake()
        {
            _flagPole.Initialize(this);
            _neighboorBuildings = new List<Building>(4) { null, null, null, null };
        }

        public void Initialize(
            FortificationManager manager,
            GridCellModel cellModel,
            float sandConcentration
        )
        {
            Initialize(manager, cellModel);
            SetHealthFromSandConcentration(sandConcentration);

            _foundationBuilder.Initialize(this);
            _foundationBuilder.Deactivate();

            ManageLinkingFoundation(-1);
        }

        #region Health

        [SerializeField]
        private AnimationCurve _qualityFromSandConcentration = null;

        private float _quality = 0f;

        [SerializeField]
        private float _qualityCoef = 100f;

        public override void InflictDamage(float damageTaken)
        {
            _flagPole.RefreshFlagHeight(damageTaken);
            base.InflictDamage(damageTaken);
        }

        public void SetHealthFromSandConcentration(float sandConcentration)
        {
            _materialPropertyBlock.SetFloat(Bucket.SAND_CONCENTRATION_PROPERTY, sandConcentration);
            _quality = _qualityFromSandConcentration.Evaluate(sandConcentration);

            _health = _maxHealth + (_quality * _qualityCoef);
            _flagPole.RefreshFlagColor(_quality);
        }

        #endregion

        #region Rempart Foundation Builder

        public void ActivateFoundationBuilder()
        {
            RefreshNeighboorBuildings();
            _foundationBuilder.Activate();
        }

        public void DeactivateFoundationBuilder()
        {
            _foundationBuilder.Deactivate();
        }

        public void ManageLinkingFoundation(int selectedSide)
        {
            int count = _linkingFoundations.Count;
            for (int i = 0; i < count; i++)
            {
                _linkingFoundations[i].SetActive(i == selectedSide);
            }
        }

        public void RefreshNeighboorBuildings()
        {
            Vector2Int coords;
            GridCellModel cellModel;
            int count = TilesetUtils.trigNeighboorsCoordinatesFour.Count;
            for (int i = 0; i < count; i++)
            {
                coords = gridCellModel.coords + TilesetUtils.trigNeighboorsCoordinatesFour[i];
                cellModel =
                    _fortificationManager.gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                        coords
                    );

                _neighboorBuildings[i] = cellModel.building;
            }
        }

        #endregion

        #endregion
    }
}
