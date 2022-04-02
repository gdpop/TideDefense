using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RempartManager))]
public class RempartManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		RempartManager _target = (RempartManager)target;

		if (GUILayout.Button("Set Rempart"))
		{
			_target.SetDebugCoord();
		}

		if (GUILayout.Button("Unset Rempart"))
		{
			_target.UnsetDebugCoord();
		}

		if (GUILayout.Button("Print Rempart"))
		{
			_target.PrintGridStatus();
		}
	}
}