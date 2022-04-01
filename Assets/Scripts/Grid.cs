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

        for (int i = 0; i< XLenght; i++)
        {
            for (int j = 0; j < YLenght; j++)
            {
                _tileList.Add(Instantiate(GridManager.Instance.PrefabTile.GetComponent<Tile>()));
            }
        }
    }
}
