using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    public float RotateValue = 10;
    public float RotateTime = 0.5f;
    public float TimeGapAfterRotate = 0.2f;
    public float StartDelay = 0.2f;
    public int Loops = -1;
    string _tweenId1;
    public string TweenId1
    {
        get
        {
            if (string.IsNullOrEmpty(_tweenId1))
                _tweenId1 = "NotifShakeAnim" + Utilities.GenerateUniqueId();

            return _tweenId1;
        }
    }
    string _tweenId2;
    public string TweenId2
    {
        get
        {
            if (string.IsNullOrEmpty(_tweenId2))
                _tweenId2 = "NotifShakeAnim2" + Utilities.GenerateUniqueId();

            return _tweenId2;
        }
    }
    private void OnEnable()
    {
        KillAnimation();
        DOTween.Sequence().AppendInterval(StartDelay).AppendCallback(() =>
        {
            DOTween.Sequence()
   .Append(transform.DORotate(new Vector3(0, 0, RotateValue * -1), RotateTime / 5f))
   .Append(transform.DORotate(new Vector3(0, 0, RotateValue), RotateTime / 5f))
   .Append(transform.DORotate(new Vector3(0, 0, RotateValue * -1), RotateTime / 5f))
   .Append(transform.DORotate(new Vector3(0, 0, RotateValue), RotateTime / 5f))
   .Append(transform.DORotate(new Vector3(0, 0, 0), RotateTime / 5f))
   .AppendInterval(TimeGapAfterRotate).SetLoops(Loops).SetId(TweenId1).OnComplete(() =>
   {
       GetComponent<ShakeAnimation>().enabled = false;
   });
        }).SetId(TweenId2);
    }

    private void OnDisable()
    {
        KillAnimation();
    }
    public void KillAnimation()
    {
        transform.rotation = Quaternion.identity;
        DOTween.Kill(TweenId1);
        DOTween.Kill(TweenId2);
    }
}
