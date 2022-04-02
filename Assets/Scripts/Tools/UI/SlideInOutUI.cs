using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlideInOutUI : UIPanel
{
    public RectTransform MainCanvas;

    public override Tweener HideAnimation()
    {
        var rt = GetComponent<RectTransform>();
        return rt.DOLocalMoveX(1900, FadeOutTime);
    }
    public override Tweener ShowAnimation()
    {
        var rt = GetComponent<RectTransform>();
        return rt.DOLocalMoveX(0, FadeInTime);

    }

}