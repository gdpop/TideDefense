using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    Sand,
    Water,
    Tower,
    Moat,
}
public class Tile : MonoBehaviour
{
    [SerializeField] TilesRendererData renderData;

    private int _xCoord;
    public int XCoord {
        get { return _xCoord; }
        set { _xCoord = value; }
    }

    private int _yCoord;
    public int YCoord {
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
        initColor = _renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHover(bool active)
    {
        if (_isHovered == active) return;
        _isHovered = active;
        //Call SetColor using the shader property name "_Color" and setting the color to red
        _renderer.material.color = active ? Color.red : initColor;
    }

    public void OnClick(bool active)
    {
        if (_isClicked == active) return;
        _isClicked = active;
        //Call SetColor using the shader property name "_Color" and setting the color to red
        _renderer.material.color = active ? Color.yellow : _isHovered ? Color.red : initColor;

        ClickOnTile(State);
    }

    private void ClickOnTile( TileState state)
    {

        switch (State)
        {
            case TileState.Sand:
                bool canBuild = SandManager.Instance.RemoveSand(-1);
                if (canBuild)
                {
                    State = TileState.Tower;

                }
                break;
            case TileState.Tower: 
                break;
            case TileState.Water: 
                break;
            case TileState.Moat: 
                break;
        }
    }
}
