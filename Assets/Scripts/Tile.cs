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

    private bool _isHovered = false;
    private bool _isClicked = false;
    private Tweener _moveTween;
    private float _initialY = 0;

    void Start()
    {
        State = TileState.Sand;
        _initialY = transform.position.y;
    }

    public void Set(TileState state)
    {
        State = state;
        switch (State)
        {
            case TileState.Sand:
                ChangeModel(0);
                break;
            case TileState.Water:
                ChangeModel(1);
                break;
            case TileState.WetSand:
                ChangeModel(2);
                break;
            case TileState.Tower:
                ChangeModel(3);
                break;
            case TileState.Moat:
                ChangeModel(4);
                break;
            case TileState.WetMoat:
                ChangeModel(5);
                break;
            default:
                ChangeModel(0);
                break;
        }
    }

    public void OnHover(bool active)
    {
        if (_isHovered == active) return;
        _isHovered = active;
        HoverEffect(active);
        //ChangeModel(active ? Color.red : initColor);
    }

    public void OnClick(bool active)
    {
        if (_isClicked == active) return;
        _isClicked = active;
        ClickEffect(active);
        //if (_isHovered) HoverEffect(active);
        //ChangeModel(active ? Color.green : _isHovered ? Color.red : initColor);
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
        for(int i = 0; i<transform.childCount; i++)
        {
            //transform.GetChild(i).gameObject.SetActive(i == stateId);
        }
    }
}
