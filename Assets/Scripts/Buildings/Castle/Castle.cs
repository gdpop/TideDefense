using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int _lifePoint;

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
}
