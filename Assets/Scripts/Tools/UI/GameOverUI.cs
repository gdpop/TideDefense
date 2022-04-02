using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIPanel
{
    public MainMenuUI MainMenu;
    public Text Score;
    public Text BestScore;
    public Text LivesLeft;
    public RectTransform FreeReviveButton;
    public RectTransform PaidReviveButton;
    //public UICircle TimerCircle;

    public bool IsReviveGame = false;
    public bool IsPaidReviveGame = false;


    private void RewardedAdCancelled()
    {
        OnContinueGame();
    }

    private void ReviveNow()
    {
        IsReviveGame = true;
        DeactivatePanel();
        MainMenu.StartGame();
    }
    public override Tweener ShowAnimation()
    {

        SetUI();
        return base.ShowAnimation();
    }
    void SetUI()
    {

    }
    void FreeReviveAnimation()
    {
        KillAllAnimation();
        var timeDuration = 5;
        var le1 = FreeReviveButton.GetComponent<LayoutElement>();
        var le2 = PaidReviveButton.GetComponent<LayoutElement>();

        le1.DOPreferredSize(Vector2.zero, 0);
        le1.DOPreferredSize(new Vector2(le2.preferredHeight, le2.preferredWidth), 0.2f);
        var rotValue = 10;
        var rotTime = 0.1f;
        DOTween.Sequence()
               //.AppendCallback(() => { VibrationManager.Instance.DoHapticImpact(MoreMountains.NiceVibrations.HapticTypes.LightImpact); })
               .Append(FreeReviveButton.DORotate(new Vector3(0, 0, rotValue * -1), rotTime))
               .Append(FreeReviveButton.DORotate(new Vector3(0, 0, rotValue), rotTime))
               .Append(FreeReviveButton.DORotate(new Vector3(0, 0, rotValue * -1), rotTime))
               .Append(FreeReviveButton.DORotate(new Vector3(0, 0, rotValue), rotTime))
               .Append(FreeReviveButton.DORotate(new Vector3(0, 0, 0), rotTime))
                                      .AppendInterval(1).SetLoops(timeDuration).SetId("ReviveLoopAnim1");
        DOTween.Sequence().AppendInterval(timeDuration).Append(le1.DOPreferredSize(Vector2.zero, 0.2f).SetEase(Ease.InBack)).SetId("ReviveLoopAnim2");
        var myFloat = 0;
        var tw = DOTween.To(() => myFloat, x => myFloat = x, timeDuration, timeDuration).SetId("ReviveLoopAnim3");
        tw.OnUpdate(() =>
        {
            //TimerCircle.SetProgress(tw.ElapsedPercentage());
        }).Play();

    }
    void KillAllAnimation()
    {
        DOTween.Kill("ReviveLoopAnim1");
        DOTween.Kill("ReviveLoopAnim2");
        DOTween.Kill("ReviveLoopAnim3");
    }
    public override Tweener HideAnimation()
    {
        KillAllAnimation();
        return base.HideAnimation();
    }


    public void OnClickFreeRevive()
    {
        DeactivateControl();
        //LaserSharpAdsManager.Instance.ShowReward(AdType.ReviveRewarded);
    }
    public void OnClickPaidRevive()
    {
        IsPaidReviveGame = true;
        DeactivatePanel();
        MainMenu.StartGame();
    }

    public void OnClickContinueButton()
    {
        OnContinueGame();
        //LaserSharpAdsManager.Instance.ShowInterstitial(AdType.Interstitial);
    }
    public void OnContinueGame()
    {
        IsReviveGame = false;
        IsPaidReviveGame = false;
        DeactivatePanel();
        MainMenu.ActivatePanel();
    }
}
