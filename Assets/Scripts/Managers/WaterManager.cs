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

    private int gridXLength;
    private int[] wave;
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


    private void Start()
    {
        StartCoroutine(Tic());

        gridXLength = GridManager.Instance.CurrentGrid.XLenght;
        wave = new int[gridXLength];
        for (int i = 0; i < wave.Length; i++)
        {
            wave[i] = 0;
        }
    }

    private void AscendingTide()
    {
        for (int i = 0; i < wave.Length; i++)
        {
            int delta = Random.Range(1, 4);
            while (delta > 0)
            {
                Tile tile = GridManager.Instance.CurrentGrid.GetTile(i, wave[i] + 1);

                switch (tile.State)
                {
                    case TileState.Sand:
                        delta--;
                        wave[i]++;
                        GridManager.Instance.CurrentGrid.SetTile(i, wave[i], TileState.Water);
                        break;
                    case TileState.Tower:
                        delta = 0;
                        break;

                }
            }

        }
    }

    private void DescendingTide()
    {
        for (int i = 0; i < wave.Length; i++)
        {
            if (GridManager.Instance.CurrentGrid.GetTile(i, wave[i - 1]))
            {
                GridManager.Instance.CurrentGrid.SetTile(i, wave[i - 1], TileState.WetSand);
                wave[i]--;
            }

        }
    }

    IEnumerator Tic()
    {
        while (true)
        {
            if (TideManager.Instance.isAscending)
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
