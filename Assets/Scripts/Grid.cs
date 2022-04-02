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
    private Vector2 _castleCoords;

    private void Awake()
    {
        Debug.Log("CastleCoords : " + LevelManager.Instance.CastleCoords);
        _castleCoords = LevelManager.Instance.CastleCoords;
        _tileList = new List<Tile>();
    }

    void Start()
    {
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

                if (i == 0)
                    SetTile(j, i, TileState.Water);

                if (GetCastleTiles(new Vector2(j, i)))
                    SetTile(j, i, TileState.Castle);
                else
                    SetTile(j, i, TileState.Sand);

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
        Debug.Log("Set Tile : " + newState);
        GetTile(coordX, coordY).Set(newState);
    }

    private bool GetCastleTiles(Vector2 currentCoords)
    {

        if (currentCoords == _castleCoords)
            Instantiate(GridManager.Instance.CastlePrefab, new Vector3(_castleCoords.x, 0, _castleCoords.y), Quaternion.identity);

        if (currentCoords.x == _castleCoords.x || currentCoords.x == (_castleCoords.x + 1) || currentCoords.x == (_castleCoords.x - 1))
        {
            if (currentCoords.y == _castleCoords.y || currentCoords.y == (_castleCoords.y + 1) || currentCoords.y == (_castleCoords.y - 1))
                return true;
        }
            
        return false;
    }
}
