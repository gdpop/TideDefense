using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextZoomIn : MonoBehaviour
{
    public Vector3 OriginalScale = Vector3.one;
    public float ZoomInTime = 0.2f;
    public float Delay = 0f;
    public Ease EaseType = Ease.InOutBack;
    bool DoAnimation = false;
    

    private void OnEnable()
    {
        if (!DoAnimation)
        {
            DoAnimation = true;
            DOTween.Sequence().SetDelay(Delay).AppendCallback(() => { transform.localScale = Vector3.zero; }).Append(transform.DOScale(OriginalScale, ZoomInTime).SetEase(EaseType)).Play().OnComplete(() => DoAnimation = false);
        }
    }

}
