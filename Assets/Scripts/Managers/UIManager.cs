using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    #region SINGLETON
    private static UIManager instance = null;

    public static UIManager Instance
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
    }
    #endregion
    #endregion

    #region Texts
    [SerializeField]
    private TextMeshProUGUI _txtSand;
    #endregion
    void Start()
    {
    }

    public void UpdateSand(int value)
    {
        _txtSand.text = string.Format("Sand: {0}/{1}", value, SandManager.Instance.MaxSandLevel);
    }
}
