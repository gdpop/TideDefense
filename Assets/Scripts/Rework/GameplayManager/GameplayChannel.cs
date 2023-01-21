namespace TideDefense
{
    using UnityEngine;
    using System;

    public delegate void RaycastEvent(RaycastHit hit);
    public delegate void ClickGrid(GridCellModel clickedCell, RaycastHit hit);

    public delegate void ToolDelegate(BeachTool tool);

    [CreateAssetMenu(
        fileName = "GameplayChannel",
        menuName = "TideDefense/GameplayChannel",
        order = 0
    )]
    public class GameplayChannel : ScriptableObject
    {

        public ClickGrid onHoverGrid = null;
        public ClickGrid onClickGrid = null;

        public ToolDelegate onChangeTool = null;
        public ToolDelegate onClickTool = null;
        public ToolDelegate onHoverTool = null;

        protected void OnEnable()
        {
            onChangeTool = (BeachTool tool) => {};
            onClickTool = (BeachTool tool) => { };
            onHoverTool = (BeachTool tool) => { };


            // onClickBeach = (RaycastHit hit) => { };

            onClickGrid = (GridCellModel clickedCell, RaycastHit hit) => { };
            
            onHoverGrid = (GridCellModel clickedCell, RaycastHit hit) => { };
        }
    }
}
