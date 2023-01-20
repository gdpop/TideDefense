using UnityEngine;
using VirtuoseReality.Extension.AudioManager;

public class SoundManagerDemo : MonoBehaviour
{

	#region Super cette histoire

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
			SoundManager.PlaySound("AmbientMusic");

		//else if(Input.GetKeyDown(KeyCode.Z))
		//	SoundManager.PlaySound(SoundDataIDStatic.U_I_BUTTON);


		//SoundManager.PlaySound(SoundDataIDStatic.TELEPORTATION)


	}

	#endregion

}
