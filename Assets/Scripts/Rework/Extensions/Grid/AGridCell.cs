namespace PierreMizzi.Grid
{
    using UnityEngine;

    public class AGridCell
    {
		#region Fields

        [SerializeField]
        protected Vector2 _coords = new Vector2();
        public Vector2 coords
        {
            get { return coords; }
            set { _coords = value; }
        }

		#endregion

		#region Methods

		#endregion
    }
}
