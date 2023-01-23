namespace TideDefense
{
    using UnityEngine;
    using PierreMizzi.MouseInteractable;
	using VirtuoseReality.Extension.AudioManager;

	public class Application : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableLayer;
        private InteractableManager _interactableManager = null;

        [SerializeField] private SoundManagerToolSettings _soundManagerSettings = null;

        [SerializeField] private Transform _soundSourceContainer = null;

        private void Start()
        {
            _interactableManager = new InteractableManager(_interactableLayer);

            SoundManager.PlaySound(SoundDataIDStatic.AMBIENT_BEACH_AND_CALM_SEA, true, 1f);
            SoundManager.PlaySound(SoundDataIDStatic.MUSIC_LIGHT_MARIMBA, true, 2f);
        }

        private void Update()
        {
            _interactableManager.Update();
        }
    }
}
