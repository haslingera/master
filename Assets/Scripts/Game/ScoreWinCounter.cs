using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWinCounter : MonoBehaviour {

	private Text _text;

	private Vector3 originalPosition;
	private int oldScore;
	private Color originalColor;
	
	// Use this for initialization
	void Start ()
	{
		_text = GetComponent<Text>();
		originalPosition = transform.GetComponent<RectTransform>().localPosition;
		oldScore = TimeScoreManager.Instance.Score;
		originalColor = _text.color;
	}
	
	// Update is called once per frame
	void Update () {

		if (TimeScoreManager.Instance.Score != oldScore)
		{
			oldScore = TimeScoreManager.Instance.Score;
			LeanTween.value(gameObject, ChangePosition, 0f, 1f, 1f).setOnComplete(ResetPosition).setEaseInOutCubic();
		}
  
	}

	private void ChangePosition(float value)
	{
		transform.GetComponent<RectTransform>().localPosition = new Vector3(originalPosition.x, originalPosition.y + 200 * value, originalPosition.z);

		_text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - value);
	}

	private void ResetPosition()
	{
		transform.GetComponent<RectTransform>().localPosition = originalPosition;
		_text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
	}
}
