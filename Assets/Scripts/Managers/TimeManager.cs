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
        StartCoroutine("TideTic");
        StartCoroutine("LifeTic");
    }

    public IEnumerator TideTic()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            isAscending = !isAscending;
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
