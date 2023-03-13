using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class OptionsUI : UIPanel
{
    public Button SoundButtonOn;
    public Button MusicButtonOn;
    public Button VibrateButtonOn;

    public Button SoundButtonOff;
    public Button MusicButtonOff;
    public Button VibrateButtonOff;

    public GameObject RemoveAdsButton;
    public GameObject RestorePurchasesButton;
    public GameObject ResetButton;
    public MainMenuUI MainMenu;
    public string LikeUsLink = @"https://www.facebook.com/advenworks/";
    public string AdvenworksLogoLink = @"http://www.advenworks.com";



    public Text RateUsButtonText;

    public override Tweener ShowAnimation()
    {
#if UNITY_IOS
        RestorePurchasesButton.Activate();
#elif UNITY_ANDROID
        RestorePurchasesButton.Deactivate();
#endif

        RemoveAdsButton.SetActive(false);
        RestorePurchasesButton.SetActive(false);
        //RateUsButtonText.SetText(LocalizedText.GetLocalizedText(string.Format("RATE {0}", Application.productName)));
        Init();

        return base.ShowAnimation();
    }

    public void Init()
    {


        //if ((SoundButtonOn) && (!PlayerDataManager.playerData.CommonData.AudioLayerToggle[LaserSharpAudioController.EAudioLayer.Sound]))
        //{
        //    SoundButtonOn.onClick.Invoke();
        //}

        //if ((MusicButtonOn) && (!PlayerDataManager.playerData.CommonData.AudioLayerToggle[LaserSharpAudioController.EAudioLayer.Music]))
        //{
        //    MusicButtonOn.onClick.Invoke();
        //}

        //if ((VibrateButtonOn) && (!PlayerDataManager.playerData.CommonData.IsVibrationOn))
        //{
        //    VibrateButtonOn.onClick.Invoke();
        //}
        //MMVibrationManager.IsVibrationEnabled = PlayerDataManager.playerData.CommonData.IsVibrationOn;

    }

    public void OnToggleSound(bool isOn)
    {
        //PlayerDataManager.playerData.CommonData.AudioLayerToggle[LaserSharpAudioController.EAudioLayer.Sound] = isOn;
        ////GameManager.Instance.audioController.UpdateAudioSettings();
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Sound", isOn.ToString());
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //if (IsPanelActive)
        //    (isOn ? SoundButtonOff : SoundButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        //else
        //    (isOn ? SoundButtonOff : SoundButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().onComplete.SafeInvoke();
        //PlayerDataManager.SavePlayerData();
    }

    public void OnToggleMusic(bool isOn)
    {
        //PlayerDataManager.playerData.CommonData.AudioLayerToggle[LaserSharpAudioController.EAudioLayer.Music] = isOn;
        ////GameManager.Instance.audioController.UpdateAudioSettings();
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Music", isOn.ToString());
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //if (IsPanelActive)
        //    (isOn ? MusicButtonOff : MusicButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        //else
        //    (isOn ? MusicButtonOff : MusicButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().onComplete.SafeInvoke();
        //PlayerDataManager.SavePlayerData();

    }
    public void OnToggleVibration(bool isOn)
    {
        //MMVibrationManager.IsVibrationEnabled = PlayerDataManager.playerData.CommonData.IsVibrationOn = isOn;
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Vibration", isOn.ToString());
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //if (IsPanelActive)
        //    (isOn ? VibrateButtonOff : VibrateButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        //else
        //    (isOn ? VibrateButtonOff : VibrateButtonOn).transform.Find("Image").gameObject.GetComponent<DOTweenAnimation>().onComplete.SafeInvoke();
        //PlayerDataManager.SavePlayerData();

    }

    public void OnClickContactUs()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //var emailDialogBuilder = new UM_EmailDialogBuilder();
        //emailDialogBuilder.SetSubject(Application.productName);
        //emailDialogBuilder.SetText(LocalizedText.GetLocalizedText("Hello!"));
        //emailDialogBuilder.AddRecipient("contact@advenworks.com");
        //UM_SocialService.SharingClient.ShowSendMailDialog(emailDialogBuilder, (x) => { });

        ////Track Button
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Contact");
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
    }
//    public void DoRate()
//    {
//        var appurl = string.Empty;
//#if UNITY_ANDROID
//        appurl = string.Format("market://details?id={0}", Application.identifier);
//#elif !UNITY_EDITOR
//        appurl = string.Format("itms-apps://itunes.apple.com/{0}?mt=8", Application.identifier);
//#endif
//        var builder = new UM_NativeDialogBuilder("", string.Format((LocalizedText.GetLocalizedText("Do you like {0} ?")), Application.productName));
//        builder.SetNegativeButton(LocalizedText.GetLocalizedText("No"), () =>
//        {

//            var emailDialogBuilder = new UM_EmailDialogBuilder();
//            emailDialogBuilder.SetSubject(string.Format(LocalizedText.GetLocalizedText("My feedback on {0}"), Application.productName));
//            emailDialogBuilder.SetText(LocalizedText.GetLocalizedText("Feedback"));
//            emailDialogBuilder.AddRecipient("contact@advenworks.com");

//            var builder2 = new UM_NativeDialogBuilder("", LocalizedText.GetLocalizedText("Do you want to tell us why?"));
//            builder2.SetNegativeButton(LocalizedText.GetLocalizedText("No"), () => { LaserSharpPlayerData.DeniedRatingInCurrentSession = true; });
//            builder2.SetPositiveButton(LocalizedText.GetLocalizedText("Yes"), () => { UM_SocialService.SharingClient.ShowSendMailDialog(emailDialogBuilder, (x) => { }); PlayerDataManager.playerData.TrackingData.IsRated = true; });

//            builder2.Build().Show();

//        });

//        builder.SetPositiveButton(LocalizedText.GetLocalizedText("Yes"), () =>
//        {

//            if (LaserSharpGameloader.CurrentGameConfig.supportRating)
//            {

//                PlayerDataManager.playerData.TrackingData.IsRated = true;
//#if UNITY_ANDROID
//                Application.OpenURL(appurl);
//#elif !UNITY_EDITOR
//                if (Device.RequestStoreReview())
//                {
//                    UM_ReviewController.RequestReview();
//                }
//                else
//                    Application.OpenURL(appurl);
//#endif
//            }
//            else
//            {
//                //Redirect to mail (typically for Low Game Config!)
//                var emailDialogBuilder = new UM_EmailDialogBuilder();
//                emailDialogBuilder.SetSubject(string.Format(LocalizedText.GetLocalizedText("My feedback on {0}"), Application.productName));
//                emailDialogBuilder.SetText(LocalizedText.GetLocalizedText("Feedback") + " <3");
//                emailDialogBuilder.AddRecipient("contact@advenworks.com");

//                var builder2 = new UM_NativeDialogBuilder("", LocalizedText.GetLocalizedText("Do you want to tell us why?"));
//                builder2.SetNegativeButton(LocalizedText.GetLocalizedText("No"), () => { LaserSharpPlayerData.DeniedRatingInCurrentSession = true; });
//                builder2.SetPositiveButton(LocalizedText.GetLocalizedText("Yes"), () => { UM_SocialService.SharingClient.ShowSendMailDialog(emailDialogBuilder, (x) => { }); PlayerDataManager.playerData.TrackingData.IsRated = true; });

//            }

//        });
//        builder.Build().Show();
//    }
    public void OnClickRateUs()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////Track Button
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Rate");
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //DoRate();





    }
    public void OnClickLikeUs()
    {
        //Track Button
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Like");
        //Application.OpenURL(LikeUsLink);
    }
    public void OnClickRestore()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Restore_iap");
        //UILoader.Instance.StartLoader();
        //LaserSharpPurchaseManager.Instance.RestorePurchases((x) =>
        //{


        //    if (x.Contains(LaserSharpGameloader.MainGameConfig.RemoveAds.GetProductId()))
        //    {
        //        PlayerDataManager.playerData.CommonData.WasPremiumUserBeforeVip = false;
        //        PlayerDataManager.playerData.CommonData.IsPremiumUser = true;
        //        PlayerDataManager.SavePlayerData();

        //        LaserSharpAdsManager.Instance.HideBannerPermanently();

        //        VibrationManager.Instance.DoHapticImpact(HapticTypes.Success);
        //    }

        //}, () =>
        //{

        //});

    }

    public void OnClickResetProgression()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Reset");
        //var builder = new UM_NativeDialogBuilder("", LocalizedText.GetLocalizedText("Restart from Level 1?"));
        //builder.SetNegativeButton(LocalizedText.GetLocalizedText("No"), () => { });
        //builder.SetPositiveButton(LocalizedText.GetLocalizedText("Yes"), () =>
        //{
        //    PlayerDataManager.ResetData();
        //    PlayerDataManager.ApplyConfigurationOverrides();
        //    PlayerDataManager.SavePlayerData();
        //});
        //builder.Build().Show();
    }

    public void OnClickAdvenworksLogo()
    {
        ////Track Button
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Logo");
        //Application.OpenURL(AdvenworksLogoLink);
    }

    public void OnClickLeaderboard()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //if (IsPanelActive)
        //    LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Settings_menu, "Leaderboard");
        //else
        //    LaserSharpAnalyticsManager.Instance.TrackButtonTap(GameAnalyticsEvents.Main_menu, "Leaderboard");

        //PerformSignInAndShowLeaderBoard();

    }
    public void PerformSignInAndShowLeaderBoard()
    {
        PerformSignIn(LeaderboardSignIn);
    }
    void LeaderboardSignIn()
    {
        //UM_GameService.LeaderboardsClient.ShowUI((result) =>
        //{
        //    if (result.IsSucceeded)
        //    {
        //        Debug.Log("User opened Leaderboards native view");
        //        PlayerDataManager.playerData.CommonData.UpdateLeaderboard = true;
        //    }
        //    else
        //    {
        //        Debug.Log("Failed to start Leaderboards native view: " + result.Error.FullMessage);
        //    }
        //});
    }
    public void PerformSignIn(Action OnSignIn)
    {


        //var signIn = true;
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    signIn = AN_GoogleApiAvailability.IsGooglePlayServicesAvailable() == AN_ConnectionResult.SUCCESS;
        //}
        //if (UM_GameService.SignInClient.PlayerInfo.State == UM_PlayerState.SignedIn)
        //    return;



    }
    public void OnClickRemoveAds()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //UILoader.Instance.StartLoader();
        //LaserSharpPurchaseManager.Instance.PurchaseProduct(LaserSharpGameloader.MainGameConfig.RemoveAds.GetProductId(), () =>
        //{
        //    UILoader.Instance.StopLoader();
        //    PlayerDataManager.playerData.CommonData.WasPremiumUserBeforeVip = true;
        //    PlayerDataManager.playerData.CommonData.IsPremiumUser = true;
        //    LaserSharpAdsManager.Instance.HideBannerPermanently();


        //}, () =>
        //{
        //    UILoader.Instance.StopLoader();
        //    Debug.Log("Purchase Failed");
        //});

    }


    public void OnCloseOptionsScreen()
    {
        //VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        ////GameManager.Instance.audioController.TriggerAudio(GameManager.uiClickTag);
        //DeactivatePanel();
        //UIManager.Instance.OpenCloseMainMenu(true);
        ////        LevelManager.Instance.UnpauseGame();
    }



}