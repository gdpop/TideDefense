using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    #region SINGLETON
    private static CastleManager instance = null;

    public static CastleManager Instance
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

    public void OnCLick()
    {

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
