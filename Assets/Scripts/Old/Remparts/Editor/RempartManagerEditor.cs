using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RempartDebugger))]
public class RempartManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		RempartDebugger _target = (RempartDebugger)target;

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

		if (GUILayout.Button("Update Rempart"))
		{
			_target.DebugUpdateMesh();
		}
	}
}