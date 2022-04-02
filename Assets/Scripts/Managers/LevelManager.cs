using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region SINGLETON
    private static LevelManager instance = null;

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
