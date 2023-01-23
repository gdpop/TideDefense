namespace TideDefense
{
	using System.Collections.Generic;
	using UnityEngine;
	
	public class SandCastle : Building {
		

		#region Health

		[SerializeField] private List<FlagPole> _flagPoles = new List<FlagPole>();

		public override void InflictDamage(float damageTaken)
        {
			foreach (FlagPole flagPole in _flagPoles)
				flagPole.RefreshFlagHeight(damageTaken);

            base.InflictDamage(damageTaken);
        }

		
		#endregion 
		
		#region Methods 

        public void Initialize(FortificationManager manager)
        {
            _fortificationManager = manager;
        }
		
		#endregion
	}
}