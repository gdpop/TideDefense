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
        protected List<Fortification> _mouldedShapes = new List<Fortification>();

        public new Fortification mouldedShape
        {
            get { return null; }
        }

		#endregion

		#region Manage Rotation
			
		#endregion

		#endregion

		#region Methods

		#region Manage rotation

		public void ManageRotation()
		{

		}

		#endregion

		#endregion
    }
}
