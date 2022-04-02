using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
	Sand,
	Water,
	WetSand,
	Tower,
	Moat,
	WetMoat,
	Castle,
	None
}

public class Tile : MonoBehaviour
{
	private int _xCoord;
	public int XCoord
	{
		get { return _xCoord; }
		set { _xCoord = value; }
	}

	private int _yCoord;
	public int YCoord
	{
		get { return _yCoord; }
		set { _yCoord = value; }
	}

	public TileState State;
	public TileState PreviousState;

	private bool _isHovered = false;
	private bool _isClicked = false;
	private Tweener _moveTween;
	private float _initialY = 0;

	private int lifePoints;

	private void Awake()
	{
		State = TileState.None;
	}

	void Start()
	{
		
		_initialY = transform.position.y;
	}

	public void Set(TileState state)
	{
		//print("avant old State:" + State + " ----> " + state);
		if (state == State)
		{
			return;
		}

		TimeManager.Instance.tick -= LooseOneLifePoint;
		PreviousState = State;
		State = state;
		switch (State)
		{
			case TileState.Sand:
				ChangeModel(0);
				lifePoints = LevelManager.Instance.SandLifePoints;
				break;
			case TileState.Water:
				ChangeModel(1);
				lifePoints = 0;
				break;
			case TileState.WetSand:
				ChangeModel(2);
				lifePoints = LevelManager.Instance.WetSandLifePoints;
				TimeManager.Instance.tick += LooseOneLifePoint;
				break;
			case TileState.Tower:
				ChangeModel(3);
				UpdateTowerModel();
				RempartManager.Instance.RefreshRempartAroundCoordinates(_xCoord, _yCoord);
				lifePoints = LevelManager.Instance.TowerLifePoints;
				break;
			case TileState.Moat:
				ChangeModel(4);
				lifePoints = LevelManager.Instance.MoatLifePoints;
				break;
			case TileState.WetMoat:
				ChangeModel(5);
				lifePoints = LevelManager.Instance.WetMoatLifePoints;
				TimeManager.Instance.tick += LooseOneLifePoint;
				break;
			case TileState.Castle:
				ChangeModel(6);
				lifePoints = 0;
				break;
			default:
				ChangeModel(0);
				break;
		}
	}

	public virtual void OnHover(bool active)
	{
		if (_isHovered == active) return;
		_isHovered = active;

		if(State == TileState.Sand)
			HoverEffect(active);
	}

	public virtual void OnLeftClick(bool active)
	{
		if (_isClicked == active) return;
		_isClicked = active;
		ClickEffect(active);
		//if (_isHovered) HoverEffect(active);
		//ChangeModel(active ? Color.green : _isHovered ? Color.red : initColor);
		if (!_isClicked)
			OnTileLeftClick();
	}

	public virtual void OnRightClick(bool active)
	{
		if (_isClicked == active) return;
		_isClicked = active;
		ClickEffect(active);
		//if (_isHovered) HoverEffect(active);
		//ChangeModel(active ? Color.green : _isHovered ? Color.red : initColor);
		if (!_isClicked)
			OnTileRightClick();
	}

	protected virtual void OnTileLeftClick()
	{
		Debug.Log("[Tile] Coords : " + XCoord + " / " + YCoord);
		switch (State)
		{
			case TileState.Sand:
				bool canBuild = SandManager.Instance.RemoveSand(SandManager.Instance.TowerPriceValue);
				if (canBuild)
				{
					GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.Tower);
				}
				break;
			case TileState.WetSand:
				break;
			case TileState.WetMoat:
				break;
			case TileState.Moat:
				break;
			case TileState.Tower:
				break;
		}
	}
	protected virtual void OnTileRightClick()
	{
		switch (State)
		{
			case TileState.Sand:
				bool canDig = SandManager.Instance.AddSand(SandManager.Instance.MoatEarnValue);
				if(canDig)
					LooseLife(1);
				break;
			case TileState.WetSand:
				break;
			case TileState.WetMoat:
				break;
			case TileState.Moat:
				break;
			case TileState.Tower:
				break;
		}
	}

	public void LooseOneLifePoint()
    {
		LooseLife(1);
	}
	public void LooseLife(int lifeInput)
    {
		if (lifeInput <= 0)
			throw new System.Exception("Invalid value");

		int newLifePoints = lifePoints -= lifeInput;

		if (newLifePoints <= 0)
			Die();
    }

	private void HoverEffect(bool active)
	{
		// Do hover effect
		transform.position = new Vector3(transform.position.x, _initialY, transform.position.z);
		_moveTween = transform.DOMoveY(_initialY + (active ? .3f : 0), .25f).SetEase(Ease.Flash);
	}
	private void ClickEffect(bool active)
	{
		// Do hover effect
		transform.position = new Vector3(transform.position.x, _initialY, transform.position.z);
		_moveTween = transform.DOMoveY(_initialY - (active ? .15f : 0), .1f).SetEase(Ease.Flash);
	}

	private void ChangeModel(int stateId)
	{
		// _renderer.material.color = color;
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i)?.gameObject.SetActive(i == stateId);//For castle tiles GetChild is null so everything is diabled
		}
	}

	private void Die()
    {
		switch (State)
		{
			case TileState.Sand:
				GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.Moat);
				break;
			case TileState.WetSand:
				GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.Sand);
				break;
			case TileState.WetMoat:
				GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.WetSand);
				break;
			case TileState.Moat:
				GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.WetMoat);
				break;
			case TileState.Tower:
				GridManager.Instance.CurrentGrid.SetTile(XCoord, YCoord, TileState.Sand);
				break;
		}
	}

	#region Tower

	private GameObject GetModel(int stateID)
	{
		return transform.GetChild(stateID).gameObject;
	}

	public void UpdateTowerModel()
	{

		Debug.Log($"REMPART : {_xCoord} : {_yCoord}");
		RempartBlock rempartBlock = RempartManager.Instance.GetRempartBlockFromCoord(_xCoord, _yCoord);
		Debug.Log("Type : " + rempartBlock.type);
		GameObject towerObject = GetModel(3);
		RempartManager.Instance.PrintGridStatus(_xCoord, _yCoord);

		MeshFilter towerMeshFilter = towerObject.GetComponent<MeshFilter>();
		towerMeshFilter.mesh = rempartBlock.mesh;

	}

	#endregion
}
