using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandManager : MonoBehaviour
{
    #region SINGLETON
    private static SandManager instance = null;

    public static SandManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]

    #endregion
    #endregion
    public int sandLevel { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddSand(int sandUnit)
    {

        if (sandUnit <= 0)
            throw new Exception("Incorrect Value");

        UpdateSandLevel(sandUnit);
    }
    public bool RemoveSand(int sandUnit)
    {
        if (sandUnit <= 0)
            throw new Exception("Incorrect Value");

        int newSandValue;
        newSandValue = sandLevel - sandUnit;

        if (newSandValue < 0)
            return false;

        UpdateSandLevel(newSandValue);
        return true;
    }

    private void UpdateSandLevel(int sandUnit)
    {
        sandLevel += sandUnit;
    }
}
