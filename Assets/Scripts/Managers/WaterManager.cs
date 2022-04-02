using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    #region SINGLETON
    private static WaterManager instance = null;

    public static WaterManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]

    private int _gridXLength;
    private int[] waveTilesYCoord;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion
    #endregion

    private int _previousFoamCoordY = 0;
    private int _foamCoordY = 5;
    public int FoamCoordY {
        get { return _foamCoordY; }
        set {
            _previousFoamCoordY = _foamCoordY;
            _foamCoordY = value; }
    }
    public void Init()
    {
        _previousFoamCoordY = FoamCoordY;
        _gridXLength = GridManager.Instance.CurrentGrid.XLenght;
        waveTilesYCoord = new int[GridManager.Instance.CurrentGrid.XLenght];
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            waveTilesYCoord[i] = FoamCoordY;
        }
        TimeManager.Instance.tideTick += UpdateTideAxis;
    }

    public void StartWater()
    {
        StartCoroutine("Tic");
    }


    private void AscendingTide(bool ascend)
    {
        //print(ascend);
        //for (int mix = 0; mix < waveTilesYCoord.Length; mix++) print(waveTilesYCoord[mix]);
        int foamAxisMax = 1;
        for (int i = 0; i < _gridXLength; i++)
        {
            Tile prevTile = GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i]);
            int delta = Random.Range(ascend ? 0 : -foamAxisMax, ascend ? foamAxisMax+1 : 1);
            //print(("delta:" + delta));
            int newY = prevTile.YCoord + delta;
            newY = Mathf.Clamp(newY, FoamCoordY-foamAxisMax, FoamCoordY +foamAxisMax);
            //print("newY" + newY);
            if (newY == prevTile.YCoord) continue;
            //print("ascend " + ascend);
            if(prevTile.YCoord < newY)
            {
                for(int indexInBetweenTiles = 0; indexInBetweenTiles < newY; indexInBetweenTiles++)
                {
                    Tile inBetweenTile = GridManager.Instance.CurrentGrid.GetTile(i , indexInBetweenTiles);
                    //print("inBetweenTile.State " + inBetweenTile.State);

                    GridManager.Instance.CurrentGrid.SetTile(i, inBetweenTile.YCoord, TileState.Water);
                }
                Tile newTile = GridManager.Instance.CurrentGrid.GetTile(i, newY);
                switch (newTile.State)
                {
                    case TileState.Tower:
                        newTile.LooseOneLifePoint();
                        continue;
                    case TileState.Castle:
                        Castle.Instance.LooseOneLifePoint();
                        continue;
                    default:
                        break;
                }
                GridManager.Instance.CurrentGrid.SetTile(i, newTile.YCoord, TileState.Water);
            }
            else
            {

                for (int ind = prevTile.YCoord; ind > newY; ind--)
                {
                    Tile newTile = GridManager.Instance.CurrentGrid.GetTile(i, ind);
                    
                    switch (newTile._previousState)
                    {
                        case TileState.Sand:
                            print("Previous State : " + newTile._previousState);
                            GridManager.Instance.CurrentGrid.SetTile(i, ind, TileState.WetSand);
                            break;
                        case TileState.Moat:
                            print("Previous State : " + newTile._previousState);
                            GridManager.Instance.CurrentGrid.SetTile(i, ind, TileState.WetMoat);
                            break;
                        default:
                            GridManager.Instance.CurrentGrid.SetTile(i, prevTile.YCoord, TileState.WetSand);
                            break;
                    }
                    
                }
                //GridManager.Instance.CurrentGrid.SetTile(i, newY, TileState.Water);
                //GridManager.Instance.CurrentGrid.SetTile(i, prevTile.YCoord, TileState.WetSand);
            }
            waveTilesYCoord[i] = newY;
        }
    }

    private void FixTilesAfterFoamYChange()
    {

    }


    private void UpdateTideAxis()
    {
        if (TimeManager.Instance.isAscending2)
            _foamCoordY++;
        else
            _foamCoordY--;

    }

    public IEnumerator Tic()
    {
        while (true)
        {
            AscendingTide(TimeManager.Instance.isAscending);
            yield return new WaitForSeconds(1f);
        }
    }

}
