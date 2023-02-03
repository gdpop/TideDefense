namespace TideDefense
{
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "TideDefense/ApplicationChannel", order = 0)]
	public class ApplicationChannel : ScriptableObject {
		
		[SerializeField]
		private bool _playTutorial = false;
		public bool playTutorial { get { return _playTutorial; } set { _playTutorial = value; } }

	}
}