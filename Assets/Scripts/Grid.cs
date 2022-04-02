using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int _xLenght;
    public int XLenght
    {
        get { return _xLenght; }
        set { _xLenght = value; }
    }

    private int _yLenght;
    public int YLenght
    {
        get { return _yLenght; }
        set { _yLenght = value; }
    }

    private List<Tile> _tileList;

    void Start()
    {
        _tileList = new List<Tile>();
    }

    public void Generate(int xLength, int yLength)
    {
        XLenght = (int)xLength;
        YLenght = (int)yLength;

        if (_tileList == null) _tileList = new List<Tile>();

        for (int i = 0; i < YLenght; i++)
        {
            for (int j = 0; j < XLenght; j++)
            {
                Tile clone = Instantiate(GridManager.Instance.PrefabTile.GetComponent<Tile>());
                clone.XCoord = j;
                clone.YCoord = i;
                clone.transform.position = new Vector3(clone.XCoord, 0, clone.YCoord);
                _tileList.Add(clone);
            }
        }
    }

    public void AddTile(Tile tile)
    {
        _tileList.Add(tile);
    }

    public void RemoveTile(Tile tile)
    {
        _tileList.Remove(tile);
    }

    // si �a retourne null c'est qu'il a pas trouv� de tile � ces coordonn�es
    public Tile GetTile(int coordX, int coordY)
    {
        foreach (Tile tile in _tileList)
        {
            if (tile.XCoord == coordX && tile.YCoord == coordY) return tile;
        }
        Debug.LogError("pas trouv� de tile � ces coordonn�es");
        return null;
    }

    public void SetTile(int coordX, int coordY, TileState newState)
    {
        GetTile(coordX, coordY).Set(newState);
    }
}
