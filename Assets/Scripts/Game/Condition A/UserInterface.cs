﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{

	public AudioSource StartAudioSource;
	public AudioSource BackgroundAudioSource;

	public GameObject StartCanvas;
	public GameObject EndCanvas;

	public Text Score;

	[HideInInspector]
	public bool IsActive;
	
	private void Start()
	{
		Cursor.visible = false;
		
		IsActive = true;
		Time.timeScale = 0;
	}

	private void Update()
	{
		
		if (StartCanvas.activeInHierarchy)
		{
			if (!Input.GetMouseButtonDown(0)) return;
			IsActive = false;
			Time.timeScale = 1f;
			StartAudioSource.Play();
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.GameStarted);
			gameObject.SetActive(false);
			StartCanvas.SetActive(false);
			EndCanvas.SetActive(true);
		}
		else
		{
			Time.timeScale = 0;
			Score.text = "SCORE " + TimeScoreManager.Instance.Score; 
			if (!Input.GetMouseButtonDown(0)) return;
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.GameEnded);
			DataRecorderNew.Instance.WriteDataToCsv();
			StartAudioSource.Play();
			SceneManager.LoadScene("_Home");
		}
		
	}

}