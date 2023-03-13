using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region SINGLETON
    private static LevelManager instance = null;

    [Header("LifePoints")]
    [SerializeField] private int _sandLifePoints;
    public int SandLifePoints { get { return _sandLifePoints; } }

    [SerializeField] private int _wetSandLifePoints;
    public int WetSandLifePoints { get { return _wetSandLifePoints; } }

    [SerializeField] private int _towerLifePoints;
    public int TowerLifePoints { get { return _towerLifePoints; } }

    [SerializeField] private int _moatLifePoints;
    public int MoatLifePoints { get { return _moatLifePoints; } }

    [SerializeField] private int _wetMoatLifePoints;
    public int WetMoatLifePoints { get { return _wetMoatLifePoints; } }

    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    [SerializeField] private Vector2 _castleCoords;
    public Vector2 CastleCoords { get { return _castleCoords; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
