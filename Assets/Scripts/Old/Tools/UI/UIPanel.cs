using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    public bool IsPanelActive;
    public bool DisableGameObjectOnDeactivation = false;
    public bool isActiveOnStart = true;

    public float FadeInTime = 0.2f;
    public float FadeOutTime = 0.2f;

    [SerializeField]
    CanvasGroup _CanvasGroup;

    public CanvasGroup GetCanvasGroup
    {
        get
        {
            if (_CanvasGroup == null)
                _CanvasGroup = GetComponent<CanvasGroup>();
            if (_CanvasGroup == null)
                _CanvasGroup = gameObject.AddComponent<CanvasGroup>();

            return _CanvasGroup;
        }
    }

    [Header("Call backs")]
    public UnityEvent OnReadyToActivate;
    public UnityEvent OnActivate;
    public UnityEvent OnReadyToDeactivate;
    public UnityEvent OnDeactivate;

    string _tweenId1;
    public string TweenIdActivate
    {
        get
        {
            if (string.IsNullOrEmpty(_tweenId1))
                _tweenId1 = "Activate" + Utilities.GenerateUniqueId();

            return _tweenId1;
        }
    }
    string _tweenId2;
    public string TweenIdDeactivate
    {
        get
        {
            if (string.IsNullOrEmpty(_tweenId2))
                _tweenId2 = "Deactivate" + Utilities.GenerateUniqueId();

            return _tweenId2;
        }
    }

    protected virtual void Awake()
    {
        if (IsActivating() || IsDeactivating())
            return;
        if (isActiveOnStart)
            ActivatePanel();
        else
            DeactivatePanel();
    }

    public virtual Tweener ShowAnimation()
    {
        return GetCanvasGroup.DOFade(1, FadeInTime).SetUpdate(UpdateType.Normal, true);
    }

    public virtual Tweener HideAnimation()
    {
        return GetCanvasGroup.DOFade(0, FadeOutTime).SetUpdate(UpdateType.Normal, true);
    }

    public void DeactivateControl()
    {
        GetCanvasGroup.interactable = false;
        GetCanvasGroup.blocksRaycasts = false;
        IsPanelActive = false;


    }

    public void ActivateControl()
    {
        GetCanvasGroup.interactable = true;
        GetCanvasGroup.blocksRaycasts = true;
        IsPanelActive = true;
    }

    public void DeactivatePanel()
    {
        DOTween.Kill(TweenIdDeactivate);
        HideAnimation().OnStart(() =>
        {
            DeactivateControl();
            //OnReadyToDeactivate.SafeInvoke();
        }).OnComplete(() =>
        {
            //if (DisableGameObjectOnDeactivation)
            //    gameObject.Deactivate();

            //OnDeactivate.SafeInvoke();

        }).SetId(TweenIdDeactivate).SetUpdate(UpdateType.Normal, true);
    }

    public virtual void ActivatePanel()
    {


        DOTween.Kill(TweenIdDeactivate);
        ShowAnimation().OnStart(() =>
        {
            //if (gameObject)
            //    gameObject.Activate();
            //OnReadyToActivate.SafeInvoke();
        })
            .OnComplete(() =>
            {
                //ActivateControl();
                //OnActivate.SafeInvoke();
            }).SetId(TweenIdActivate).SetUpdate(UpdateType.Normal, true);

    }

    public bool IsDeactivating()
    {
        return DOTween.IsTweening(TweenIdDeactivate);
    }
    public bool IsActivating()
    {
        return DOTween.IsTweening(TweenIdActivate);
    }
    public void TogglePanel()
    {
        if (IsPanelActive)
            DeactivatePanel();
        else
            ActivatePanel();
    }
    public void TogglePanel(bool isOn)
    {
        if (isOn)
            ActivatePanel();
        else
            DeactivatePanel();
    }

}