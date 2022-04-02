using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BlinkText : MonoBehaviour {

    private TextMeshProUGUI text;
    //private Color currentColor;
    private bool increase;

    public float blinkSpeed = 1.0f;

	// Use this for initialization
	void Start () {

        text = GetComponent<TextMeshProUGUI>();
        //currentColor = text.color;
        //increase = false;
        increase = true;
        Color newColor = text.color;
        newColor.a = 0.0f;
        text.color = newColor;

        //DOTween.To(() => currentColor.a, x => currentColor.a = x, 0.0f, blinkSpeed).OnUpdate(() => { text.color = currentColor; Debug.Log("TMP:" + currentColor.a); });

	}

    // Update is called once per frame
    private Color newColor;
	void Update () {

        if(text.enabled) {

            newColor = text.color;
            if((increase) && (newColor.a>=1.0f)) {
                increase = false;
            } else if ((!increase) && (newColor.a <= 0.0f)) {
                increase = true;
            }

            if(increase) {
                newColor.a += blinkSpeed * Time.deltaTime;
            } else {
                newColor.a -= blinkSpeed * Time.deltaTime;
            }

            text.color = newColor;

            //Debug.Log("TMP:" + newColor.a);

        }
	}
}
