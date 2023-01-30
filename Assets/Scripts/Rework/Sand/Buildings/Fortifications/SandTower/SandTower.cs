namespace TideDefense
{
    using System.Collections.Generic;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    public class SandTower : Fortification
    {
        #region Fields

        #region Flag Pole

        [SerializeField]
        private FlagPole _flagPole = null;

        #endregion

        #region Linking Remparts

        private List<Building> _neighboorBuildings = new List<Building>();
        

        [SerializeField]
        private List<GameObject> _linkingRemparts = new List<GameObject>();

        #endregion

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            _flagPole.Initialize(this);
            _neighboorBuildings = new List<Building>(4) { null, null, null, null };
        }

        public override void Initialize(
            FortificationManager manager,
            GridCellModel cellModel,
            float sandConcentration
        )
        {
            base.Initialize(manager, cellModel, sandConcentration);
            // ManageLinkingRemparts(-1);
        }

        #region Health

        public override void InflictDamage(float damageTaken)
        {
            _flagPole.RefreshFlagHeight(damageTaken);
            base.InflictDamage(damageTaken);
        }

        protected override void SetHealthFromSandConcentration(float sandConcentration)
        {
            base.SetHealthFromSandConcentration(sandConcentration);
            _flagPole.RefreshFlagColor(_quality);
        }

        #endregion

        #region Linking Remparts

        public void ManageLinkingRemparts(int selectedSide)
        {
            int count = _linkingRemparts.Count;
            for (int i = 0; i < count; i++)
            {
                _linkingRemparts[i].SetActive(i == selectedSide);
            }
        }

        #endregion

        #endregion
    }
}
