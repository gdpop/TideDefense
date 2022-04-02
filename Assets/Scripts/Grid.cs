using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private uint _xLenght;
    public uint XLenght {
        get { return _xLenght; }
        set { _xLenght = value; }
    }

    private uint _yLenght;
    public uint YLenght {
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
        XLenght = (uint)xLength;
        YLenght = (uint)yLength;

        if(_tileList == null) _tileList = new List<Tile>();

        for (int i = 0; i< YLenght; i++)
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

    // si ça retourne null c'est qu'il a pas trouvé de tile à ces coordonnées
    public Tile GetTile(int coordX, int coordY)
    {
        foreach(Tile tile in _tileList)
        {
            if (tile.XCoord == coordX && tile.YCoord == coordY) return tile;
        }
        Debug.LogError("pas trouvé de tile à ces coordonnées");
        return null;
    }
}
