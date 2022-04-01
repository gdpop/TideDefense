using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

    #region [ FIELDS ]

    #region SINGLETON
    private static Singleton instance = null;
    
    public static Singleton Instance {
        get {
            return instance;
        }
    }
    #endregion

    #endregion

    #region [ METHODS ]

    #region [ MONOBEHAVIOR ]
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    
    void Start() {

    }
    
    void Update() {

    }
    #endregion

    #endregion



}