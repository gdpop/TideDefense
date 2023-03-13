using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextZoomOutIn : MonoBehaviour
{
    public Vector3 LargeZoomValue = new Vector3(2, 2, 2);
    public Vector3 OriginalScale = Vector3.zero;
    public float ZoomInTime = 0.2f;
    public float TextFadeInTime = 0.2f;
    public float TextFadeOutTime = 0.2f;
    public float Delay = 0f;
    public bool DoFadeout = false;
    public Ease EaseType = Ease.InOutBack;
    bool DoAnimation = false;
    string _tweenId;
    public string TweenId
    {
        get
        {
            if (string.IsNullOrEmpty(_tweenId))
                _tweenId = "TextZoomOutIn"  + Utilities.GenerateUniqueId();

            return _tweenId;
        }
    }
    private void Awake()
    {
        if (OriginalScale == Vector3.zero)
            OriginalScale = transform.localScale;
    }

    private void OnEnable()
    {
        if (!DoAnimation)
        {
            DoAnimation = true;
            var t = GetComponent<Text>();
            if (t != null) t.DOFade(0, 0);
            transform.localScale = LargeZoomValue;
            var seq = DOTween.Sequence().Append(transform.DOScale(OriginalScale, ZoomInTime).SetEase(EaseType));
            if (DoFadeout)
            {
                seq.Insert(Mathf.Max(TextFadeInTime, ZoomInTime), GetComponent<Text>().DOFade(0, TextFadeOutTime).OnComplete(() =>
                     {
                         //gameObject.Deactivate();
                     }));
            }
            seq.OnComplete(() => DoAnimation = false);
            if (t != null)
                seq.Insert(0, GetComponent<Text>().DOFade(1, TextFadeInTime));
            seq.SetDelay(Delay);
            seq.Play().SetId(TweenId);
        }
    }
    private void OnDisable()
    {
        DoAnimation = false;
        transform.localScale = LargeZoomValue;
        DOTween.Kill(TweenId);
        var t = GetComponent<Text>();
        if (t != null)
            GetComponent<Text>().DOFade(0, 0);
    }

}
