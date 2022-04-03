using TMPro;
using UnityEngine;

public class Bucket : MonoBehaviour
{

	#region Fields

	[SerializeField] private TextMeshPro _text = null;

	[SerializeField] private Material _sandBucketMaterial = null;

	private const string k_sandLevel = "_SandLevel";

	[Range(0f, 1f)] private float _sandLevel = 0f;

	#endregion

	#region Methods

	private void Update()
	{
		RefreshSandBucketLevel(_sandLevel);
	}

	public void RefreshSandBucketLevel(float normalized)
	{
		_sandBucketMaterial.SetFloat(k_sandLevel, _sandLevel);
	}

	public void RefreshText(string text)
	{
		_text.text = text;
	}

	#endregion

}