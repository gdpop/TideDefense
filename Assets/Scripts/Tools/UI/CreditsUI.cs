using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : UIPanel
{
    public GameObject CreditsRollPanel;
    public Text CreditsText;
    string creditsString = null;
    [Range(0.0f, 100.0f)]
    public float CreditsRollTime;

    string GetCredits {
        
        get {
            var r = Resources.Load<TextAsset>("Credits/Credits" + "_" + Application.systemLanguage.ToString());
            if (r == null)
                r = Resources.Load<TextAsset>("Credits/Credits_English");

            creditsString = r.text;
            return creditsString;
        }
    }

    public void OnActivatePanel()
    {
        //CreditsText.SetText(GetCredits);
        Canvas.ForceUpdateCanvases();
        var rt = CreditsRollPanel.GetComponent<RectTransform>();
        GetComponent<RectTransform>().sizeDelta = new Vector2(transform.root.GetComponent<RectTransform>().rect.width - transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().sizeDelta.y);
        rt.DOLocalMoveY(rt.rect.size.y + (GetComponent<RectTransform>().rect.size.y / 2), CreditsRollTime).SetLoops(-1, LoopType.Restart).SetSpeedBased(true).SetId("CreditsRoll");
    }
    public void OnDectivatePanel()
    {

        DOTween.Kill("CreditsRoll");

        CreditsRollPanel.GetComponent<RectTransform>().DOMoveY(0, 0);
    }

}