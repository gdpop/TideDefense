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
        attackTick = () => { };
    }
    #endregion
    #endregion

    public bool isAscending = true;
    public bool isAscending2 = true;

    public Action tick;
    public Action tideTick;
    public Action attackTick;

    private void Start()
    {
        StartCoroutine("FoamTic");
        StartCoroutine("LifeTic");
        StartCoroutine("TideTick");
        StartCoroutine("AttackTick");
    }

    public IEnumerator FoamTic()
    {
        while (true)
        {
            //yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
            yield return new WaitForSeconds(6f);
            isAscending = !isAscending;
            //WaterManager.Instance.FoamCoordY += isAscending ? 1 : -1;
        }
    }

    public IEnumerator TideTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            tideTick.Invoke();
        }
    }

    public IEnumerator AttackTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            attackTick.Invoke();
            isAscending2 = !isAscending2;
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
