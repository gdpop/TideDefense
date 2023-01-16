namespace TideDefense
{
    using UnityEngine;
    using System;

    public delegate void OnRaycastEvent(RaycastHit hit);
    public delegate void OnClickGrid(Vector2Int coords);

    public delegate void OnChangeTool(ToolType toolType);

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


        public OnChangeTool onChangeTool = null;

        public Action onClickBucket = null;

        protected void OnEnable()
        {
            onChangeTool = (ToolType toolType) => {};
            onClickBucket = () => { };


            onClickBeach = (RaycastHit hit) => { };
            onHoverBeach = (RaycastHit hit) => { };
            onClickGrid = (Vector2Int coords) => { };
        }
    }
}
