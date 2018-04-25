using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScoreManager : MonoBehaviour
{
	public static TimeScoreManager Instance;
	public GameObject Menu;

	public float GameTimeMinutes = 2.5f;

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
		RemainingGameTime -= Time.deltaTime;

		if (RemainingGameTime <= 0)
		{
			Menu.SetActive(true);
		}
	}

	public void IncreaseScore()
	{
		Score++;
	}
}
