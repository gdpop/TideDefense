using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIDropDown : MonoBehaviour
{
    public float OpenTime = 1f;
    public float CloseTime = 1f;
    public float FadeInTime = 0.5f;
    public float FadeOutTime = 0.5f;
    public GameObject DropDownIcon;
    public GameObject DropDownSubstituteIcon;
    public RectTransform DropDownContainer;
    public RectTransform DropDownBG;
    public Ease OpenEase;
    public Ease CloseEase;
    public CanvasGroup DropDownCanvas;
    public bool IsOpen = false;
    float OnCloseHeight;
    float OnOpenHeight;
    GraphicRaycaster inputRaycaster;
    void Awake()
    {
        inputRaycaster = GetComponent<GraphicRaycaster>();
        GetHeights();
        var openTimeBackup = OpenTime;
        var closeTimeBackup = CloseTime;
        var fadeinTimeBackup = FadeInTime;
        var fadeoutTimeBackup = FadeOutTime;
        OpenTime = CloseTime = FadeInTime = FadeOutTime = 0;
        if (IsOpen)
            OnClickDropDownIcon();
        else
            OnCloseDropDownIcon();

        OpenTime = openTimeBackup;
        CloseTime = closeTimeBackup;
        FadeInTime = fadeinTimeBackup;
        FadeOutTime = fadeoutTimeBackup;
    }
    private void Update()
    {
        if (IsOpen && Input.GetMouseButtonDown(0))
        {
            IsOpen = false;
            DOTween.Sequence().AppendInterval(0.2f).AppendCallback(OnCloseDropDownIcon);
        }
    }
    void ToggleDropDown(bool isOpen)
    {
        DropDownSubstituteIcon.SetActive(isOpen);
        DropDownIcon.SetActive(!isOpen);
    }
    void GetHeights()
    {
        OnCloseHeight = GetComponent<RectTransform>().rect.size.y + DropDownContainer.GetComponent<VerticalLayoutGroup>().padding.top + Mathf.Abs(DropDownBG.GetComponent<RectTransform>().anchoredPosition3D.y);
        OnOpenHeight = DropDownBG.GetComponent<RectTransform>().rect.size.y;
    }

    public void OnClickDropDownIcon()
    {
        if (DOTween.IsTweening("OnDropDown"))
            return;
        ToggleDropDown(true);
        //GameManager.Instance.audioController.TriggerAudio("OnClick");
        inputRaycaster.enabled = false;
        DropDownBG.GetComponent<CanvasGroup>().DOFade(1, FadeInTime).OnComplete(() =>
         {
             DropDownBG.DOSizeDelta(new Vector2(DropDownBG.sizeDelta.x, OnOpenHeight), OpenTime).OnComplete(() =>
             {
                 inputRaycaster.enabled = true;
                 IsOpen = true;

             }).SetId("OnDropDown").SetEase(OpenEase);
             DropDownCanvas.DOFade(1, OpenTime);
         }).SetId("OnDropDown");


    }
    public void OnCloseDropDownIcon()
    {
        if (DOTween.IsTweening("OnDropDown"))
            return;
        inputRaycaster.enabled = false;
        ToggleDropDown(false);
        DropDownBG.DOSizeDelta(new Vector2(DropDownBG.sizeDelta.x, OnCloseHeight), CloseTime).OnComplete(() =>
        {
            inputRaycaster.enabled = true;
            DropDownBG.GetComponent<CanvasGroup>().DOFade(0, FadeOutTime).SetId("OnDropDown");
            IsOpen = false;
        }).SetId("OnDropDown").SetEase(CloseEase);
        DropDownCanvas.DOFade(0, CloseTime);
    }
}
