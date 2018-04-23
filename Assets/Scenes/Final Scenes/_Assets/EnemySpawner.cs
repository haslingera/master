using Game;
using UnityEngine;
using UnityEngine.UI;

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
	public float BufferSpaceshipThreshold;
	public float SpawnInset = 0.9f;
	public float SpawnDistanceMin = 50f;
	public float SpawnDistanceMax = 80f;
	
	[Header("Gaze Direction Buffer")]
	public float GazeBuffer;

	[Header("Debug Settings")]
	public Image DebugImage;

	private GameObject _currentEnemy;
	private float _time;
	private float _currentWaitTime;
	private float _buffer = 2;
	private float _gazeBuffer;
	private Vector3 _enemyPositionToBe;
	private bool _positionSet;

	void Start()
	{
		_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
	}

	void Update () {
		
		if (_time >= _currentWaitTime && SpaceshipControls.GetSpaceShipTurn().x < SpaceshipHorizontalThreshold && SpaceshipControls.GetSpaceShipTurn().y < SpaceshipVerticalThreshold && !_currentEnemy && _buffer > BufferSpaceshipThreshold)
		{
			SpawnEnemy();
			_currentWaitTime = Random.Range(WaitTimeBetweenEnemySpawnMin, WaitTimeBetweenEnemySpawnMax);
			_time = 0;
			_buffer = 0;
		}

		if (SpaceshipControls.GetSpaceShipTurn().x >= SpaceshipHorizontalThreshold ||
		    SpaceshipControls.GetSpaceShipTurn().y >= SpaceshipVerticalThreshold)
		{
			_buffer = 0;
		}

		_buffer += Time.deltaTime;
		_time += Time.deltaTime;
			
	}

	void SpawnEnemy()
	{
		if (_currentEnemy == null)
		{
			_enemyPositionToBe = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width * SpawnInset), Random.Range(0, Screen.height * SpawnInset), Random.Range(SpawnDistanceMin, SpawnDistanceMax)));
			_currentEnemy = Instantiate(Enemy, _enemyPositionToBe, Quaternion.identity);
		}
	}

	public GameObject GetCurrentEnemy()
	{
		return _currentEnemy;
	}

}
