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

		for (int i = 0; i < _neighboorsCoordinates.Count; i++)
		{
			Vector2 offset = _neighboorsCoordinates[i];
			Tile tile = GridManager.Instance.CurrentGrid.GetTile(x + (int)offset.x, y + (int)offset.y);

			bitmask += (tile.State == TileState.Tower) ? "1" : "0";
		}

		// converting to integer
		Debug.Log(bitmask);
		int enumValue = Convert.ToInt32(bitmask, 2);
		Debug.Log(enumValue);

		return enumValue;
	}

	public RempartBlock GetRempartBlockFromCoord(int x, int y)
	{
		TilesetType type = (TilesetType)GetRempartNeighboors(x, y);

		RempartBlock block = _rempartBlocks.Find(item => item.type == type);

		if (block.mesh != null)
			return block;
		else
		{
			Debug.LogWarning("Couldn't GetRempartMeshFromCoord");
			return new RempartBlock();
		}
	}

	[field: SerializeField]
	public int myGetter
	{
		get; set;
	}

}