using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class RempartManager : MonoBehaviour
{

	#region Fields

	[SerializeField] private List<List<bool>> _gridRempartStatus = new List<List<bool>>();

	[SerializeField] private int _width = 3;
	[SerializeField] private int _height = 3;

	[SerializeField] private int _debugX = 0;
	[SerializeField] private int _debugY = 0;

	private List<Vector3> _neighboorsCoordinates = new List<Vector3>();

	[SerializeField] private List<RempartBlock> _rempartBlocks = new List<RempartBlock>();


	#endregion

	#region Methods

	private void OnEnable()
	{
		InitializeGrid();

		SetRempart(1, 1);
		Debug.Log(GetRempart(1, 1));

		PrintGridStatus();
	}

	public void InitializeGrid()
	{
		_gridRempartStatus = new List<List<bool>>(_width);

		for (int x = 0; x < _width; x++)
		{
			List<bool> _heights = new List<bool>(_height);

			for (int y = 0; y < _height; y++)
			{
				_heights.Add(false);
				// Debug.Log($" {x} : {y}");
			}
			_gridRempartStatus.Add(_heights);

		}

		_neighboorsCoordinates = new List<Vector3>(8)
		{
			new Vector2(0,1),
			new Vector2(1,1),
			new Vector2(1,0),
			new Vector2(1,-1),
			new Vector2(0,-1),
			new Vector2(-1,-1),
			new Vector2(-1,0),
			new Vector2(-1,1),
		};

	}

	public void SetRempart(int x, int y)
	{
		if (CheckValidCoordinates(x, y))
			_gridRempartStatus[x][y] = true;

	}

	public void UnsetRempart(int x, int y)
	{
		if (CheckValidCoordinates(x, y))
			_gridRempartStatus[x][y] = false;
	}

	public bool GetRempart(int x, int y)
	{
		if (CheckValidCoordinates(x, y))
			return _gridRempartStatus[x][y];
		else
			return false;
	}

	public bool CheckValidCoordinates(int x, int y)
	{
		if (x < 0 || _width - 1 < x)
		{
			Debug.LogWarning($"{x}:{y} : {x} is wrong");
			return false;
		}

		if (y < 0 || _height - 1 < y)
		{
			Debug.LogWarning($"{x}:{y} : {y} is wrong");
			return false;
		}

		return true;
	}


	/// <summary> 
	///	Clockwise from the top, get a bitmask of neight	
	///</summary>
	public void GetRempartNeighboors(int x, int y)
	{
		string bitmask = "";

		for (int i = 0; i < _neighboorsCoordinates.Count; i++)
		{
			Vector2 offset = _neighboorsCoordinates[i];
			bitmask += GetRempart(x + (int)offset.x, y + (int)offset.y) ? "1" : "0";
		}

		// converting to integer
		Debug.Log(bitmask);
        int enumValue = Convert.ToInt32(bitmask, 2);
		Debug.Log(enumValue);
	}

	#region Debugs

	[SerializeField, TextArea(4, 4)] private string _gridStatus = "";

	[ContextMenu("Set DebugCoord")]
	public void SetDebugCoord()
	{
		SetRempart(_debugX, _debugY);
		PrintGridStatus();
		GetRempartNeighboors(1, 1);
	}

	[ContextMenu("Unset DebugCoord")]
	public void UnsetDebugCoord()
	{
		UnsetRempart(_debugX, _debugY);
		PrintGridStatus();
		GetRempartNeighboors(1, 1);
	}

	[ContextMenu("Print Grid Status")]
	public void PrintGridStatus()
	{
		string gridPrint = "";
		for (int y = _height - 1; y >= 0; y--)
		{
			string row = "";
			for (int x = 0; x < _width; x++)
				row += _gridRempartStatus[x][y] ? "1" : "0";

			gridPrint += row + "\r\n";
		}

		Debug.Log(gridPrint);
		_gridStatus = gridPrint;

	}

	[ContextMenu("Get Rempart Neighboors")]
	public void GetRempartNeighboors()
	{
		GetRempartNeighboors(_debugX, _debugY);

	}

	#endregion

	#endregion

}
