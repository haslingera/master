using System;
using Game;
using Gaze;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

	[Header("Scripts")]
	public SpaceshipControls SpaceshipControls;
	public GameObject Enemy;

	[Header("Spaceship Dependent Parameters")]
	public float SpaceshipHorizontalThreshold = 5f;
	public float SpaceshipVerticalThreshold = 5f;
	
	[Header("Enemy Spawn Parameters")]
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

	private float _fovealRadius;

	void Start()
	{
		_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
		_fovealRadius = CalculateFovealRadiusInPixel();
	}

	void Update () {
		
		if (!_currentEnemy && _time >= _currentWaitTime && _buffer > EnemySpawnSafetyThreshold && IsSpaceShipTurningTooMuch())
		{
			SpawnEnemy();
			_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
			_time = 0;
			_buffer = 0;
		}

		CalculateEnemySpawnSafetyThreshold();
		
		if (!_currentEnemy) _time += Time.deltaTime;
			
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

		Vector3 screenPointEnemyPosition = new Vector3(Random.Range(0, Screen.width * SpawnInset), Random.Range(0, Screen.height * SpawnInset), Random.Range(SpawnDistanceMin, SpawnDistanceMax));

		while (!IsDistanceToGazeEnough(screenPointEnemyPosition))
		{
			screenPointEnemyPosition = new Vector3(Random.Range(0, Screen.width * SpawnInset), Random.Range(0, Screen.height * SpawnInset), Random.Range(SpawnDistanceMin, SpawnDistanceMax));
		}
				
		_enemyPositionToBe = Camera.main.ScreenToWorldPoint(screenPointEnemyPosition);
		_currentEnemy = Instantiate(Enemy, _enemyPositionToBe, Quaternion.identity);
		
		_currentEnemy.GetComponent<Enemy>().SetHideTimeAndInvokeFlash(GazeDirectionTime);
		Invoke("ShowEnemy", GazeDirectionTime);
	}

	float CalculateFovealRadiusInPixel ()
	{
		float distanceToComputerSquared = GazeManager.Instance.DistanceToComputer * GazeManager.Instance.DistanceToComputer;
		float radiusCm = Mathf.Sqrt(distanceToComputerSquared + distanceToComputerSquared - 2f * distanceToComputerSquared * Mathf.Cos(GazeManager.Instance.FovealVisionRadians));
		float radiusPx = Screen.dpi * (radiusCm / 2.54f);
		return radiusPx / 2f;
	}
	
	
	private bool IsDistanceToGazeEnough(Vector3 pointToDisplay)
	{
		return Vector2.Distance(new Vector2(pointToDisplay.x, pointToDisplay.y), GazeManager.Instance.SmoothGazeVector) > _fovealRadius;
	}

	void ShowEnemy()
	{
		_currentEnemy.GetComponent<Enemy>().ShowEnemy();
	}

}
