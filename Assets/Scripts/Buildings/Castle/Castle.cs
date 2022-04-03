using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CastleUpgrades {
    public int level;
    public GameObject model;
    public int nextUpgradeValue;
}

public class Castle : Tile
{
    #region SINGLETON
    private static Castle instance = null;

    public static Castle Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    [SerializeField] private int maxLifePoints;

    [SerializeField] private CastleUpgrades[] upgrades;

    int _lifePoint;
    int _castleLevel = 0;
    int _upgradePoints = 0;

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


    public override void OnHover(bool active)
    {
        return;
    }

    public override void OnLeftClick(bool active)
    {
        base.OnLeftClick(active);
    }

    public override void OnRightClick(bool active)
    {
        return;
    }

    protected override void OnTileLeftClick()
    {
        bool canUpgrade = SandManager.Instance.RemoveSand(SandManager.Instance.CastleUpgradeValue);
        if (canUpgrade)
        {
            AddUpgradePoint(1);
            AddLifePoint(SandManager.Instance.CastleUpgradeValue);
        }
    }
    protected override void OnTileRightClick()
    {
        
        return;
    }

    public void AddLifePoint(int lifeInput)
    {
        if (lifeInput <= 0)
            throw new System.Exception("Invalid input value");

        int newLifePoints = _lifePoint += lifeInput;

        if (newLifePoints >= maxLifePoints)
            return;

        UpdateCastle(lifeInput);
    }
    public void RemoveLifePoint(int lifeInput)
    {
        if (lifeInput <= 0)
            throw new System.Exception("Invalid input value");

        int newLifePoints = _lifePoint += lifeInput;

        if (newLifePoints <= 0)
        {
            GameManager.Instance.EndGame();
            return;
        }

        UpdateCastle(lifeInput);
    }

    private void UpdateCastle(int lifeInput)
    {
        _lifePoint += lifeInput;
    }

    private void AddUpgradePoint(int value) {
        _upgradePoints += value;
        Debug.Log("Add Upgrade Point");

        if(CanUpgrade()) UpgradeCastle();
    }

    private bool CanUpgrade() {
        if(_castleLevel >= upgrades.Length - 1) return false;
        else {
            if(_upgradePoints >= upgrades[_castleLevel].nextUpgradeValue) return true;
            else return false;
        }
    }

    private void UpgradeCastle() {
        Debug.Log("Upgrading castle to level " + (_castleLevel + 1));
        upgrades[_castleLevel].model.SetActive(false);
        _castleLevel++;
        upgrades[_castleLevel].model.SetActive(true);
        _upgradePoints = 0;
    }
}
