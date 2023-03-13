using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GDPRPopUpUI : UIPanel
{
    protected static bool IsSpawnedAlready = false;
    protected static GDPRPopUpUI _instance = null;

    public static GDPRPopUpUI Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GDPRPopUpUI>();
            if (_instance == null)
            {

                var go = Instantiate(Resources.Load("GDPRPanel")) as GameObject;
                _instance = go.GetComponent<GDPRPopUpUI>();
                _instance.gameObject.name = _instance.GetType().Name;

            }

            IsSpawnedAlready = true;
            return _instance;
        }
        set { _instance = value; }
    }
    public string PrivacyPolicyLink = @"https://advenworks.com/privacy-policy/";
    public TextMeshProUGUI GameName;
    public TextMeshProUGUI Message1;
    protected override void Awake()
    {


        if (IsSpawnedAlready)
        {
            Destroy(gameObject);
        }

        if (transform.root == transform)
            DontDestroyOnLoad(this);

        IsSpawnedAlready = true;
        GameName.SetText(Application.productName);
        Message1.SetText(string.Format("As indie developers, we keep {0} free by showing you ads.", Application.productName));
        base.Awake();
    }
    public void OnRecieveGDPRConsent()
    {
        DeactivatePanel();
        //PlayerDataManager.playerData.TrackingData.HasPlayerGivenGDPRConsent = true;
        //PlayerDataManager.SavePlayerData();
        //LaserSharpAdsManager.Instance.DoInitStuff();
    }
    public void OnClickPrivacyPolicyLink()
    {
        Application.OpenURL(PrivacyPolicyLink);
    }
}