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


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        tick = () => { };
        tideTick = () => { };
    }
    #endregion
    #endregion

    public bool isAscending = true;

    public Action tick;
    public Action tideTick;

    private void Start()
    {
        StartCoroutine("FoamTic");
        StartCoroutine("LifeTic");
        StartCoroutine("TideTick");
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

    public IEnumerator TideTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            tideTick.Invoke();
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
