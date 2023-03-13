using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public class ButtonOnClick : MonoBehaviour {
	public float ScaleFactor = 1.1f;
	public float Speed = 0.05f;
	void Awake () {
		GetComponent<Button> ().onClick.AddListener (OnClickAnimation);
	}

	private void OnClickAnimation () {
		transform.DOScale (transform.localScale * ScaleFactor, Speed).SetLoops (2, LoopType.Yoyo);
	}
}