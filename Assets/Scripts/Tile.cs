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

    // juste pour le debug
    private Renderer _renderer;

    public TileState State;

    Color initColor;

    private bool _isHovered = false;
    private bool _isClicked = false;
    void Start()
    {
        State = TileState.Sand;
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        //initColor = _renderer.material.color;
        initColor = Color.yellow;
        ChangeColor(Color.yellow);
    }

    public void Set(TileState state)
    {
        State = state;
        switch (State)
        {
            case TileState.Water:
                ChangeColor(Color.blue);
                break;
            case TileState.WetSand:
                ChangeColor(Color.cyan);
                break;
            case TileState.Tower:
                ChangeColor(Color.black);
                break;
            default:
                ChangeColor(Color.yellow);
                break;
        }
    }

    public void OnHover(bool active)
    {
        if (_isHovered == active) return;
        _isHovered = active;
        //Call SetColor using the shader property name "_Color" and setting the color to red
        ChangeColor(active ? Color.red : initColor);
    }

    public void OnClick(bool active)
    {
        if (_isClicked == active) return;
        _isClicked = active;
        //Call SetColor using the shader property name "_Color" and setting the color to red
        ChangeColor(active ? Color.green : _isHovered ? Color.red : initColor);
    }

    private void ChangeColor(Color color)
    {
        // _renderer.material.color = color;
        gameObject.SetActive(color == Color.yellow);
    }
}
