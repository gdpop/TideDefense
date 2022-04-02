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

    private uint gridXLength;
    private uint[] wave;
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
        wave = new uint[gridXLength];
        for (int i = 0; i < wave.Length; i++)
        {
            wave[i] = 0;
        }
    }

    IEnumerator Tic()
    {
        while (true)
        {
            for (int i = 0; i < wave.Length; i++)
            {
                // GridManager.Instance.GetTileByCoord()
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
