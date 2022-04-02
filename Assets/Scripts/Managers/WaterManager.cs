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

    private int _foamCoordY = 5;
    public int FoamCoordY {
        get { return _foamCoordY; }
        set { _foamCoordY = value; }
    }
    public void Init()
    {
        _gridXLength = GridManager.Instance.CurrentGrid.XLenght;
        waveTilesYCoord = new int[GridManager.Instance.CurrentGrid.XLenght];
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            waveTilesYCoord[i] = FoamCoordY;
        }
    }

    public void StartWater()
    {
        StartCoroutine("Tic");
    }


    private void AscendingTide(bool ascend)
    {
        print(ascend);
        for (int mix = 0; mix < waveTilesYCoord.Length; mix++) print(waveTilesYCoord[mix]);
        int foamAxisMax = 2;
        for (int i = 0; i < _gridXLength; i++)
        {
            print(i + "//" + _gridXLength);
            Tile prevTile = GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i]);
            int delta = Random.Range(ascend ? 0 : -foamAxisMax - 1, ascend ? foamAxisMax+1 : 0);
            print(("delta:" + delta));
            int newY = prevTile.YCoord + delta;
            newY = Mathf.Clamp(newY, FoamCoordY-foamAxisMax, FoamCoordY +foamAxisMax);
            print("newY" + newY);
            if (newY == waveTilesYCoord[i]) continue;
            
            Tile nextTile = GridManager.Instance.CurrentGrid.GetTile(i, newY);

            if(ascend)
            {
                switch (nextTile.State)
                {
                    case TileState.Sand:
                        GridManager.Instance.CurrentGrid.SetTile(i, newY, TileState.Water);
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
                        GridManager.Instance.CurrentGrid.SetTile(i, newY, TileState.Water);
                        //waveTilesYCoord[i] = newY;
                        break;
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

    public IEnumerator Tic()
    {
        while (true)
        {
            AscendingTide(TimeManager.Instance.isAscending);
            yield return new WaitForSeconds(1f);
        }
    }

}
