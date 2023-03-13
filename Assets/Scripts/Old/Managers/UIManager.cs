using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Slider _slider;
    void Start()
    {
    }

    public void UpdateSand(float value)
    {
        float maxSandLevel = SandManager.Instance.MaxSandLevel;
        float percentage = value / maxSandLevel;
        _slider.value = percentage;

        _txtSand.text = string.Format("Sand: {0}/{1}", value, SandManager.Instance.MaxSandLevel);
    }
}
