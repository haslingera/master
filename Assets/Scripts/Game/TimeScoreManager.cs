using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScoreManager : MonoBehaviour
{
	public static TimeScoreManager Instance;
	public GameObject Menu;

	public float GameTimeMinutes = 2.5f;
	public bool GameHasTime = true;

	[HideInInspector]
	public int Score;

	[HideInInspector]
	public float RemainingGameTime;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance != this)
		{
			Destroy(gameObject);
		}

		RemainingGameTime = GameTimeMinutes * 60;
	}

	private void Update()
	{
		if (GameHasTime)
		{
			RemainingGameTime -= Time.deltaTime;

			if (Menu != null && RemainingGameTime <= 0)
			{
				Menu.SetActive(true);
			}
		}
		
	}

	public void IncreaseScore()
	{
		Score++;
	}
}
