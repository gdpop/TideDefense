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

    [Header("Sand Parameters"), SerializeField] int _maxSandLevel;
    [SerializeField] private int _towerPriceValue;
    [SerializeField] private int _castleUpgradeValue;
    [SerializeField] private int _moatEarnValue;

    public int MaxSandLevel { get { return _maxSandLevel; } }
    public int TowerPriceValue { get { return _towerPriceValue; } }
    public int CastleUpgradeValue { get { return _castleUpgradeValue; } }
    public int MoatEarnValue { get { return _moatEarnValue; } }

    public IntDelegate onUpdateSandLevel = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;

        onUpdateSandLevel = (int value)=>{};
    }
    

    public bool AddSand(int sandUnit)
    {

        if (sandUnit <= 0)
            throw new Exception("Incorrect Value");

        if (sandLevel == _maxSandLevel)
            return false;

        int newSandValue;
        newSandValue = sandLevel + sandUnit;

        if(newSandValue > _maxSandLevel)
        {
            sandLevel = _maxSandLevel;
            return false;
        }

        UpdateSandLevel(sandUnit);
        return true;
    }

    public bool RemoveSand(int sandUnit)
    {
        if (sandUnit <= 0)
            throw new Exception("Incorrect Value");

        int newSandValue;
        newSandValue = sandLevel - sandUnit;

        if (newSandValue < 0)
            return false;

        UpdateSandLevel(-sandUnit);
        return true;
    }

    private void UpdateSandLevel(int sandUnit)
    {
        int newSandLevel = sandLevel + sandUnit;
        sandLevel = newSandLevel;
        onUpdateSandLevel.Invoke(sandLevel);
        UIManager.Instance.UpdateSand(newSandLevel);
    }
}
