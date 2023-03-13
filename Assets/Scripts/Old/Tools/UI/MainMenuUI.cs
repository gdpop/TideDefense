using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuUI : UIPanel
{
    public RectTransform FooterContainer;
    public RectTransform AdsPanel;

    public OptionsUI OptionsScreen;
    public GameOverUI GameOverScreen;

    public List<GameObject> VipButtons = new List<GameObject>();

    public Text BestScore;

    public GameObject RewardedVideoIcon;
    public GameObject InterstitialAdIcon;
    public void OnClickRewarded()
    {
        RewardedVideoIcon.SetActive(false);
        //LaserSharpAdsManager.Instance.ShowReward( Placement.Rwd_Shop);
    }
    public void OnClickInterstitial()
    {
        InterstitialAdIcon.SetActive(false);
        //LaserSharpAdsManager.Instance.ShowInterstitial(Placement.Int_OnEndingLevel);
    }

    public void SetBannerAdCallBacks()
    {
        //LaserSharpAdsManager.OnBannerShow.RemoveAllListeners();
        //LaserSharpAdsManager.OnBannerShow.AddListener(OnBannerShow);

        //LaserSharpAdsManager.OnBannerHide.RemoveAllListeners();
        //LaserSharpAdsManager.OnBannerHide.AddListener(OnBannerFail);

        //LaserSharpAdsManager.OnBannerFailed.RemoveAllListeners();
        //LaserSharpAdsManager.OnBannerFailed.AddListener(OnBannerFail);

        //LaserSharpAdsManager.OnInterstitialLoaded.RemoveAllListeners();
        //LaserSharpAdsManager.OnInterstitialLoaded.AddListener(() =>
        //{
        //    //InterstitialAdIcon.SetActive(true);
        //});

        //LaserSharpAdsManager.OnRewardedAdLoaded.RemoveAllListeners();
        //LaserSharpAdsManager.OnRewardedAdLoaded.AddListener(() =>
        //      {
        //          //RewardedVideoIcon.SetActive(true);
        //      });
    }

    protected override void Awake()
    {
        base.Awake();
        //RewardedVideoIcon.SetActive(LaserSharpAdsManager.Instance.IsRewardedReady());
        //InterstitialAdIcon.SetActive(LaserSharpAdsManager.Instance.IsInterstitialReady());
        // int bestScore = PlayerPrefs.GetInt("BestScore");
        // BestScore.SetText(bestScore);
    }

    private void OnBannerFail()
    {
        FooterContainer.DOAnchorPosY(0f, 0f);
        //LaserSharpAdsManager.Instance.HideBanner();
    }

    private void OnBannerShow()
    {
        FooterContainer.DOAnchorPosY(200f, 0f);
        //AdsPanel.Activate();
    }

    public override Tweener ShowAnimation()
    {
        OptionsScreen.Init();
        SetBannerAdCallBacks();

        // TO DECOMMENT TO ENABLE BANNERS


        //LaserSharpAdsManager.Instance.ShowBannerWhenReady();
        //if (LaserSharpPurchaseManager.IsConnected && LaserSharpGameloader.CurrentGameConfig.isVIPSupported)
        //    VipButtons.Each((x, i) => { x.SetActive(!PlayerDataManager.playerData.CommonData.IsVIPUser); });
        return base.ShowAnimation();
    }
    public override Tweener HideAnimation()
    {
        if (IsPanelActive)
            HideBannerAd();
        return base.HideAnimation();
    }

    public void HideBannerAd()
    {
        //LaserSharpAdsManager.Instance.HideBanner();
    }

    public void StartGame()
    {
        DeactivatePanel();
        //LaserSharpAdsManager.Instance.GamePlayIncrementation();
        //GameManager.Instance.InitGame();
        //DOTween.Sequence().AppendInterval(5f).AppendCallback(() =>
        //{
        //    GameOverScreen.ActivatePanel();
        //});
    }
    public void OpenSettings()
    {
        DeactivatePanel();
        OptionsScreen.ActivatePanel();
    }
    public void OpenShop()
    {
        // DeactivatePanel();
        //ShopScreen.ActivatePanel();
    }
}
