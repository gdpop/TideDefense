using System.Collections.Generic;
using TMPro;
using UnityEngine;


// TODO : Sand Bar
// TODO : Sand Counter
// TODO : Available Rempart

public class ConstructionVisuals : MonoBehaviour
{

	#region Fields


	#endregion

	#region MonoBehaviour

	private void Start()
	{
		Initialize();

		if (SandManager.Instance != null)
			SandManager.Instance.onUpdateSandLevel += CallbackUpdateSandLevel;
	}

	private void Destroy()
	{
		if (SandManager.Instance != null)
			SandManager.Instance.onUpdateSandLevel -= CallbackUpdateSandLevel;
	}



	#endregion

	#region Behaviour


	private void CallbackUpdateSandLevel(int sandLevel)
	{
		UpdateSandBucket(sandLevel);
		UpdateSandLevelText(sandLevel);
		UpdateAvailbleRempart(sandLevel);
	}


	private void Initialize()
	{
		UpdateSandBucket(0);
		UpdateSandLevelText(0);
		UpdateAvailbleRempart(0);
	}
	#endregion

	#region Sand Level

	[SerializeField] private Transform _sandBucket = null;

	[SerializeField] private Vector2 _minMaxSandBucketScale = new Vector2();
	private float _sandBucketScale = 0f;
	private float _normalizedSandLevel = 0f;
	public void UpdateSandBucket(int sandLevel)
	{
		_normalizedSandLevel = (float)sandLevel / (float)SandManager.Instance.MaxSandLevel;
		_sandBucketScale = Mathf.Lerp(_minMaxSandBucketScale.x, _minMaxSandBucketScale.y, _normalizedSandLevel);
		_sandBucket.localScale = new Vector3(1, _sandBucketScale, 1);
	}

	#endregion

	#region Sand Counter
	[SerializeField] private TextMeshPro _sandCounterText = null;

	public void UpdateSandLevelText(int sandLevel)
	{
		_sandCounterText.text = sandLevel.ToString();
	}

	#endregion

	#region Available Rempart

	private int availableRemparts = 0;

	[SerializeField] private Transform _availableTowersContainer = null;


	public void UpdateAvailbleRempart(int sandLevel)
	{
		availableRemparts = (int)Mathf.Floor(sandLevel / SandManager.Instance.TowerPriceValue);

		for (int i = 0; i < _availableTowersContainer.childCount; i++)
		{
			GameObject tower = _availableTowersContainer.GetChild(i).gameObject;
			tower.SetActive(i < availableRemparts);
		}

	}

	#endregion
}