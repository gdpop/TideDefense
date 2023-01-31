using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class Bucket : MouldTool
    {
		#region Fields

        [Header("Mould")]

        [SerializeField]
        protected MouldShape _mouldedShape;

		public override MouldShape mouldedShape
        {
            get { return _mouldedShape; }
        }

		#endregion

		#region Methods



		#endregion
    }
}
