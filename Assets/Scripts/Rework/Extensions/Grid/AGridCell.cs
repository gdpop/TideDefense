namespace PierreMizzi.Grid
{
    using UnityEngine;

    public class AGridCell
    {
		#region Fields

        [SerializeField]
        protected Vector2Int _coords = new Vector2Int();
        public Vector2Int coords
        {
            get { return _coords; }
            set { _coords = value; }
        }

		#endregion

		#region Methods

		#endregion
    }
}
