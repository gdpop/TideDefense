using System.Collections.Generic;
using UnityEngine;

public class OffsetTool : MonoBehaviour
{
	[SerializeField] private Vector3 _offset = new Vector3();

	[SerializeField] private List<Transform> _transforms = new List<Transform>();

	[ContextMenu("label")]
	public void AddOffset()
	{
		foreach (Transform items in _transforms)
		{
			items.position += _offset;
		}
	}
}