namespace TideDefense
{
    using UnityEngine;
    using System;
    using PierreMizzi.MouseInteractable;

    public delegate void RaycastEvent(RaycastHit hit);
    public delegate void ClickGrid(GridCellModel clickedCell, RaycastHit hit);
    public delegate void GridCellModelDelegate(GridCellModel cellMode);

    public delegate void ToolDelegate(BeachTool tool);

    [CreateAssetMenu(
        fileName = "GameplayChannel",
        menuName = "TideDefense/GameplayChannel",
        order = 0
    )]
    public class GameplayChannel : ScriptableObject
    {
        public BoolDelegate onSetActiveSphericalCamera = null;

        // Click Grid
        public ClickGrid onClickGrid = null;

        // Hold Click Grid
        public Action OnStartHoldClickGrid;
        public FloatEvent OnProgressHoldClickGrid;
        public GridCellModelDelegate OnCompleteHoldClickGrid;
        public Action OnCancelHoldClickGrid;

        // Hover Grid
        // public ClickGrid onHoverGrid = null;

        public RaycastDelegate onHoverBeach = null;

        public ToolDelegate onChangeTool = null;
        public ToolDelegate onClickTool = null;
        public ToolDelegate onHoverTool = null;

        protected void OnEnable()
        {
            onSetActiveSphericalCamera = (bool isActive)=>{};

            onChangeTool = (BeachTool tool) => { };
            onClickTool = (BeachTool tool) => { };
            onHoverTool = (BeachTool tool) => { };

            onClickGrid = (GridCellModel clickedCell, RaycastHit hit) => { };

            OnStartHoldClickGrid = () => { };
            OnProgressHoldClickGrid = (float value) => { };
            OnCompleteHoldClickGrid = (GridCellModel clickedCell) => { };
            OnCancelHoldClickGrid = () => { };

            onHoverBeach = (RaycastHit hit) => { };

            // onHoverGrid = (GridCellModel clickedCell, RaycastHit hit) => { };
        }
    }
}
