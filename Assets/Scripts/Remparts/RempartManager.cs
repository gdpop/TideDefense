using System;
using System.Collections.Generic;
using UnityEngine;

public class RempartManager : ATilesetManager
{

	#region SINGLETON
	private static RempartManager instance = null;

	public static RempartManager Instance
	{
		get
		{
			return instance;
		}
	}

	#region [ MONOBEHAVIOR ]

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;
	}

	#endregion

	#endregion

	[SerializeField] private List<RempartBlock> _rempartBlocks = new List<RempartBlock>();

	public override bool CheckValidCoordinates(int x, int y)
	{
		return base.CheckValidCoordinates(x, y);
	}

	public int GetRempartNeighboors(int x, int y)
	{
		string bitmask = "";

		for (int i = 0; i < _neighboorsCoordinatesFour.Count; i++)
		{
			Vector2 offset = _neighboorsCoordinatesFour[i];
			Tile tile = GridManager.Instance.CurrentGrid.GetTile(x + (int)offset.x, y + (int)offset.y);

			if (tile == null)
			{
				bitmask += 0;
				continue;
			}

			bitmask += (tile.State == TileState.Tower) ? "1" : "0";
		}

		// converting to integer
		// Debug.Log(bitmask);
		int enumValue = Convert.ToInt32(bitmask, 2);
		// Debug.Log(enumValue);

		return enumValue;
	}

	public RempartBlock GetRempartBlockFromCoord(int x, int y)
	{
		TilesetTypeFour type = (TilesetTypeFour)GetRempartNeighboors(x, y);

		RempartBlock block = _rempartBlocks.Find(item => item.type == type);

		if (block.mesh != null)
			return block;
		else
		{
			Debug.LogWarning("Couldn't GetRempartMeshFromCoord");
			return new RempartBlock();
		}
	}


	public void RefreshRempartAroundCoordinates(int x, int y)
	{
		Debug.Log("RefreshRempartAroundCoordinates");
		for (int i = 0; i < _neighboorsCoordinatesFour.Count; i++)
		{
			Vector2 offset = _neighboorsCoordinatesFour[i];
			Vector2 neighboorCoord = new Vector2(x + (int)offset.x, y + (int)offset.y);
			Tile tile = GridManager.Instance.CurrentGrid.GetTile((int)neighboorCoord.x, (int)neighboorCoord.y);

			if (tile == null)
				continue;

			if (tile.State == TileState.Tower)
				tile.UpdateTowerModel();
		}

	}

	[SerializeField] private MeshFilter _meshFilter = null;

	[SerializeField] private TilesetTypeEight _type = TilesetTypeEight.Empty;

	// [ContextMenu("Test MeshFilter")]
	// public void TestMeshFilter()
	// {
	// 	_meshFilter.mesh = _rempartBlocks.Find(item => item.type == _type).mesh;
	// }

	[ContextMenu("Print Grid Status")]
	public void PrintGridStatus(int x, int y)
	{
		string gridPrint = tonFils;

		for (int i = 0; i < _neighboorsCoordinatesFour.Count; i++)
		{
			Vector2 offset = _neighboorsCoordinatesFour[i];
			Vector2 neighboorCoord = new Vector2(x + (int)offset.x, y + (int)offset.y);
			Tile tile = GridManager.Instance.CurrentGrid.GetTile((int)neighboorCoord.x, (int)neighboorCoord.y);

			if (tile == null)
			{
				gridPrint = gridPrint.Replace($"({i})", "0");
				continue;
			}

			gridPrint = gridPrint.Replace($"({i})", tile.State == TileState.Tower ? "1" : "0");
		}
		Debug.Log(gridPrint);
		// _gridStatus = gridPrint;

	}


	private string tonFils = "(7)(0)(1)\r\n(6)X(2)\r\n(5)(4)(3)";
}