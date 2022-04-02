using System.Collections.Generic;
using UnityEngine;

public abstract class ATilesetManager : MonoBehaviour
{

	protected List<Vector3> _neighboorsCoordinates = new List<Vector3>();

	protected virtual void OnEnable()
	{
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


	private int width
	{
		get
		{
			if (GridManager.Instance != null && GridManager.Instance.CurrentGrid != null)
				return GridManager.Instance.CurrentGrid.XLenght;
			else
			{
				Debug.LogError($"GridManager not initialized");
				return 0;
			}
		}
	}

	private int height
	{
		get
		{
			if (GridManager.Instance != null && GridManager.Instance.CurrentGrid != null)
				return GridManager.Instance.CurrentGrid.YLenght;
			else
			{
				Debug.LogError($"GridManager not initialized");
				return 0;
			}
		}
	}



	public virtual bool CheckValidCoordinates(int x, int y)
	{
		if (x < 0 || width - 1 < x)
		{
			Debug.LogWarning($"{x}:{y} : {x} is wrong");
			return false;
		}

		if (y < 0 || height - 1 < y)
		{
			Debug.LogWarning($"{x}:{y} : {y} is wrong");
			return false;
		}

		return true;
	}



	// public virtual int

}