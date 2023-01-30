using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class Bucket : MouldTool
    {
		#region Fields

        [Header("Mould")]

        [SerializeField]
        protected Fortification _mouldedShape = null;

		public new Fortification mouldedShape
        {
            get { return _mouldedShape; }
        }

		#endregion

		#region Methods



		#endregion
    }
}
