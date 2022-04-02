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


    public void Init()
    {
        _gridXLength = GridManager.Instance.CurrentGrid.XLenght;
        waveTilesYCoord = new int[_gridXLength];
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            waveTilesYCoord[i] = 0;
        }
    }

    public void StartWater()
    {
        StartCoroutine("Tic");
    }

    private void AscendingTide()
    {
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            int delta = Random.Range(1, 3);

            for (int j = 0; j < delta; j++)
            {
                Tile nextTile = GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i] + 1);

                switch (nextTile.State)
                {
                    case TileState.Sand:
                        GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.Water);
                        waveTilesYCoord[i]++;
                        break;
                    case TileState.Tower:
                        break;
                    case TileState.Moat:
                        GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.Water);
                        break;
                    default:
                        GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.Water);
                        waveTilesYCoord[i]++;
                        break;
                }
            }

        }
    }

    private void DescendingTide()
    {
        for (int i = 0; i < waveTilesYCoord.Length; i++)
        {
            if (GridManager.Instance.CurrentGrid.GetTile(i, waveTilesYCoord[i]) != null)
            {
                GridManager.Instance.CurrentGrid.SetTile(i, waveTilesYCoord[i], TileState.WetSand);
                waveTilesYCoord[i]--;
            }

        }
    }

    public IEnumerator Tic()
    {
        while (true)
        {
            if (TimeManager.Instance.isAscending)
            {
                AscendingTide();
            }
            else
            {
                DescendingTide();
            }


            yield return new WaitForSeconds(1f);
        }
    }

}
