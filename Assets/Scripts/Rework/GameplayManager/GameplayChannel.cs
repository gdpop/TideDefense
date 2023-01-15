namespace TideDefense
{
    using UnityEngine;

    public delegate void OnClickBeach(RaycastHit hit);
    public delegate void OnClickGrid(Vector2Int coords);

    [CreateAssetMenu(
        fileName = "GameplayChannel",
        menuName = "TideDefense/GameplayChannel",
        order = 0
    )]
    public class GameplayChannel : ScriptableObject
    {
        public OnClickBeach onClickBeach = null;
        public OnClickGrid onClickGrid = null;

        protected void OnEnable()
        {
            onClickBeach = (RaycastHit hit) => { };
            onClickGrid = (Vector2Int coords) => { };
        }
    }
}
