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
        print(ascend);
        for (int mix = 0; mix < waveTilesYCoord.Length; mix++) print(waveTilesYCoord[mix]);
        int foamAxisMax = 1;
        for (int i = 0; i < _gridXLength; i++)
        {
            print(i + "//" + _gridXLength);
            Tile prevTile = GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i]);
            int delta = Random.Range(ascend ? 0 : -foamAxisMax, ascend ? foamAxisMax+1 : 1);
            print(("delta:" + delta));
            int newY = prevTile.YCoord + delta;
            newY = Mathf.Clamp(newY, FoamCoordY-foamAxisMax, FoamCoordY +foamAxisMax);
            print("newY" + newY);
            if (newY == waveTilesYCoord[i]) continue;

            if(ascend)
            {
                for(int indexInBetweenTiles = 1; indexInBetweenTiles <= delta; indexInBetweenTiles++)
                {
                    Tile inBetweenTile = GridManager.Instance.CurrentGrid.GetTile(i , prevTile.YCoord+indexInBetweenTiles);

                    switch (inBetweenTile.State)
                    {
                        case TileState.Sand:
                            GridManager.Instance.CurrentGrid.SetTile(i, inBetweenTile.YCoord, TileState.Water);
                            //waveTilesYCoord[i] = newY;
                            break;
                        case TileState.Tower:
                            // Ici on met un dégât à la tour et on return;
                            break;
                        case TileState.Moat:
                            // Ici on met un dégât à la tour et on return; 
                            break;
                        default:
                            //GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.Water);
                            //if (i < waveTilesYCoord.Length - 1)  waveTilesYCoord[i]++;
                            GridManager.Instance.CurrentGrid.SetTile(i, inBetweenTile.YCoord, TileState.Water);
                            //waveTilesYCoord[i] = newY;
                            break;
                    }
                }

            }
            else
            {

                for(int ind = prevTile.YCoord; ind > newY; ind--) GridManager.Instance.CurrentGrid.SetTile(i, ind, TileState.WetSand);
                //GridManager.Instance.CurrentGrid.SetTile(i, prevTile.YCoord, TileState.WetSand);
                GridManager.Instance.CurrentGrid.SetTile(i, newY, TileState.Water);
            }
            waveTilesYCoord[i] = newY;
        }
    }

    private void FixTilesAfterFoamYChange()
    {

    }

    private void DescendingTide()
    {
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            if (GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i]) != null)
            {
                GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.WetSand);
                if(i>=1) waveTilesYCoord[i]--;
            }

        }
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
