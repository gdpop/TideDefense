using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TideManager : MonoBehaviour
{
    #region SINGLETON
    private static TideManager instance = null;

    public static TideManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]

    public bool isAscending = true;

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
        StartCoroutine("Tic");
    }

    public IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            isAscending = !isAscending;
        }
    }


}
