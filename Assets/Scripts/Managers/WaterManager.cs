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

    private void RisingTide()
    {
        for (int i = 0; i < wave.Length; i++)
        {
            Tile tile = GridManager.Instance.CurrentGrid.GetTile(i, wave[i + 1]);

            // switch (tile.GetType)
            // {

            //     default:
            // }
        }
    }

    private void RecedingTide()
    {
        for (int i = 0; i < wave.Length; i++)
        {
            Tile tile = GridManager.Instance.CurrentGrid.GetTile(i, wave[i + 1]);

            // switch (tile.GetType)
            // {

            //     default:
            // }
        }
    }

    IEnumerator Tic()
    {
        while (true)
        {
            if (TimeManager.Instance.isRecedingTide)
            {
                RecedingTide();
            }

            if (TimeManager.Instance.isRisingTide)
            {
                RisingTide();
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
