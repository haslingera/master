using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeFormatter : MonoBehaviour
{
	private Text _text;

	// Use this for initialization
	void Start ()
	{
		_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_text.text =  (int) (TimeScoreManager.Instance.RemainingGameTime / 60) + ":" + String.Format("{0:0}", (int) (TimeScoreManager.Instance.RemainingGameTime % 60));
	}
}
