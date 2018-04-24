using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScoreManager : MonoBehaviour
{
	public static TimeScoreManager Instance;

	public float GameTime = 300;

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

		RemainingGameTime = GameTime;
	}

	private void Update()
	{
		RemainingGameTime -= Time.deltaTime;
	}

	public void IncreaseScore()
	{
		Score++;
	}
}
