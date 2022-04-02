using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region SINGLETON
    private static TimeManager instance = null;

    public static TimeManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]

    public bool isAscending = true;

    public Action tick;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        tick = () => { };
    }
    #endregion
    #endregion


    private void Start()
    {
        StartCoroutine("FoamTic");
        StartCoroutine("LifeTic");
    }

    public IEnumerator FoamTic()
    {
        while (true)
        {
            //yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
            yield return new WaitForSeconds(2f);
            isAscending = !isAscending;
            //WaterManager.Instance.FoamCoordY += isAscending ? 1 : -1;
        }
    }

    public IEnumerator LifeTic()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            tick.Invoke();
        }
    }
}
