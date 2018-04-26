using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFormatter : MonoBehaviour {

	private Text _text;

	// Use this for initialization
	void Start ()
	{
		_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = String.Format("{0:000}", TimeScoreManager.Instance.Score);    
	}
}
