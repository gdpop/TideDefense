using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class RempartMould : MouldTool
    {
		#region Fields

		#region Mould

        [Header("Mould")]
        [SerializeField]
        private int _mouldShapeIndex = 0;
        public int mouldShapeIndex
        {
            get { return _mouldShapeIndex; }
            set
            {
                if (0 <= value && value < _mouldedShapes.Count)
                    _mouldShapeIndex = value;
                else
                    Debug.LogError("For me, that's impossible");
            }
        }

        [SerializeField]
        protected List<MouldShape> _mouldedShapes = new List<MouldShape>();
        public List<MouldShape> mouldedShapes
        {
            get { return _mouldedShapes; }
        }

        public override MouldShape mouldedShape
        {
            get { return _mouldedShapes[_mouldShapeIndex]; }
        }

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour


		#endregion

		#endregion
    }
}
