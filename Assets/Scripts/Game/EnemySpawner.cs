using System;
using System.CodeDom;
using System.Linq.Expressions;
using Game;
using Gaze;
using Guidance;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

	[Header("Scripts")]
	public SpaceshipControls SpaceshipControls;
	public GameObject Enemy;

	[Header("Spaceship Dependent Parameters")]
	[Range(0f, 5f)]
	public float SpaceshipHorizontalThreshold = 5f;
	[Range(0f, 5f)]
	public float SpaceshipVerticalThreshold = 5f;

	[Header("Enemy Spawn Parameters")]
	public float FirstWaitTime;
	public float WaitTimeBetweenEnemySpawnMin = 5f;
	public float WaitTimeBetweenEnemySpawnMax = 10f;
	public float SpawnInset = 0.8f;
	public float SpawnDistanceMin = 50f;
	public float SpawnDistanceMax = 80f;
	
	[Header("Spawn Safety Threshold")]
	public float EnemySpawnSafetyThreshold;
	
	[Header("Gaze Direction Buffer")]
	public float GazeDirectionTime;

	private GameObject _currentEnemy;
	private float _time;
	private float _currentWaitTime;
	private float _buffer = 2;
	private Vector3 _enemyPositionToBe;
	private bool _positionSet;
	private float _currentFirstWaitTime;

	private SubtleGazeDirection _subtleGazeDirection;

	void Start()
	{
		_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
		_subtleGazeDirection = GameObject.Find("Gaze Guidance").GetComponent<SubtleGazeDirection>();
	}

	void Update () {

		if (_currentFirstWaitTime > FirstWaitTime)
		{
			if (!_currentEnemy && _time >= _currentWaitTime && _buffer > EnemySpawnSafetyThreshold &&
			    IsSpaceShipTurningTooMuch())
			{
				SpawnEnemy();
				_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
				_time = 0;
				_buffer = 0;
			}

			CalculateEnemySpawnSafetyThreshold();

			if (!_currentEnemy) _time += Time.deltaTime;
		}
		else
		{
			_currentFirstWaitTime += Time.deltaTime;
		}

	}

	bool IsSpaceShipTurningTooMuch()
	{
		return SpaceshipControls.GetSpaceShipTurn().x < SpaceshipHorizontalThreshold && SpaceshipControls.GetSpaceShipTurn().y < SpaceshipVerticalThreshold;
	}

	void CalculateEnemySpawnSafetyThreshold()
	{
		if (SpaceshipControls.GetSpaceShipTurn().x >= SpaceshipHorizontalThreshold || SpaceshipControls.GetSpaceShipTurn().y >= SpaceshipVerticalThreshold) _buffer = 0;
		_buffer += Time.deltaTime;
	}

	void SpawnEnemy()
	{
		if (_currentEnemy != null) return;

		Vector3 screenPointEnemyPosition = new Vector3(Random.Range(Screen.width - Screen.width * SpawnInset, Screen.width * SpawnInset), Random.Range(Screen.height - Screen.height * SpawnInset, Screen.height * SpawnInset), Random.Range(SpawnDistanceMin, SpawnDistanceMax));
		
		if (GazeManager.Instance.GazeAvailable) {
			while (true)
			{
				if (IsDistanceToGazeEnough(screenPointEnemyPosition) && !IsInsideSpaceShip(screenPointEnemyPosition))
				{
					break;
				}
			
				screenPointEnemyPosition = new Vector3(Random.Range(Screen.width - Screen.width * SpawnInset, Screen.width * SpawnInset), Random.Range(Screen.height - Screen.height * SpawnInset, Screen.height * SpawnInset), Random.Range(SpawnDistanceMin, SpawnDistanceMax));
			}
			
		}

		lastX = screenPointEnemyPosition.x;
		lastY = screenPointEnemyPosition.y;
				
		_enemyPositionToBe = Camera.main.ScreenToWorldPoint(screenPointEnemyPosition);
		_currentEnemy = Instantiate(Enemy, _enemyPositionToBe, Quaternion.identity);
		
		Invoke("ShowEnemy", GazeDirectionTime);
	}

	private bool IsInsideSpaceShip(Vector3 pointToDisplay)
	{
		Rect rect = new Rect(0.3857f * Screen.width, 0.1708f * Screen.height, 0.2285f * Screen.width, 0.1927f * Screen.height);
		
		if (rect.Contains(new Vector2(pointToDisplay.x, pointToDisplay.y)))
		{
			return true;
		}

		return false;
	}

	private float lastX;
	private float lastY;
	
	
	private bool IsDistanceToGazeEnough(Vector3 pointToDisplay)
	{
		
		if (lastX < Screen.width / 2f)
		{
			if (pointToDisplay.x < Screen.width / 2f)
			{
				return false;
			}
		}
		
		if ( lastX >= Screen.width / 2f)
		{
			if (pointToDisplay.x >= Screen.width / 2f)
			{
				return false;
			}
		}
		
		if (lastY < Screen.height / 2f)
		{
			if (pointToDisplay.y < Screen.height / 2f)
			{
				return false;
			}
		}
		
		if ( lastY >= Screen.height / 2f)
		{
			if (pointToDisplay.y >= Screen.height / 2f)
			{
				return false;
			}
		}
				
		return Vector2.Distance(new Vector2(pointToDisplay.x, pointToDisplay.y), GazeManager.Instance.SmoothGazeVector) > _subtleGazeDirection.PerceptualSpanPixel + _subtleGazeDirection.ModulationRadiusPixel;
	}

	void ShowEnemy()
	{
		if (_currentEnemy != null)
		{
			_currentEnemy.GetComponent<Enemy>().ShowEnemy();
		}
	}

}
