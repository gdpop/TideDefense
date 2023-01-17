namespace TideDefense
{
    using UnityEngine;
    using System;

    public delegate void OnRaycastEvent(RaycastHit hit);
    public delegate void OnClickGrid(GridCell clickedCell, RaycastHit hit);

    public delegate void ToolDelegate(BeachTool tool);

    [CreateAssetMenu(
        fileName = "GameplayChannel",
        menuName = "TideDefense/GameplayChannel",
        order = 0
    )]
    public class GameplayChannel : ScriptableObject
    {
        public OnRaycastEvent onClickBeach = null;
        public OnRaycastEvent onHoverBeach = null;

        public OnClickGrid onClickGrid = null;


        public ToolDelegate onChangeTool = null;

        public ToolDelegate onClickTool = null;
        public ToolDelegate onHoverTool = null;

        protected void OnEnable()
        {
            onChangeTool = (BeachTool tool) => {};
            onClickTool = (BeachTool tool) => { };
            onHoverTool = (BeachTool tool) => { };


            onClickBeach = (RaycastHit hit) => { };
            onHoverBeach = (RaycastHit hit) => { };
            onClickGrid = (GridCell clickedCell, RaycastHit hit) => { };
        }
    }
}
