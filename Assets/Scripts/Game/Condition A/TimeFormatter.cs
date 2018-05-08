using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeFormatter : MonoBehaviour
{
	private Text _text;

	void Start ()
	{
		_text = GetComponent<Text>();
	}
	
	void Update () {
		_text.text =  (int) (TimeScoreManager.Instance.RemainingGameTime / 60) + ":" + ((int) (TimeScoreManager.Instance.RemainingGameTime % 60)).ToString("00");
	}
}
