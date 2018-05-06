using Game;
using Guidance;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GazeCalibrationA : MonoBehaviour
{

	public SpaceshipControls _SpaceshipControls;
	public FireLaser _FireLaser;
	public Crosshair _Crosshair;
	public GameObject EnemySpawner;

	public GameObject[] TestObjects;

	private int _counter;
	private SubtleGazeDirection _sgd;
	
	// Use this for initialization
	void Start ()
	{

		Cursor.visible = false;
		_sgd = GameObject.Find("Gaze Guidance").GetComponent<SubtleGazeDirection>();
		_sgd.ModulationIntensityMin = 0;
		_sgd.ModulationIntensityMax = 0;
		
		IntensityManager.Instance.ResetIntensities();

		if (TestObjects.Length > 0)
		{
			TestObjects[0].AddComponent<PointOfInterest>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (_counter == 0)
			{
				
				IntensityManager.Instance.AddGazeIntensitySampleMin(_sgd.ModulationIntensityMin);
				Destroy(TestObjects[_counter]);
				_counter++;
				TestObjects[_counter].AddComponent<PointOfInterest>();
				
				_sgd.ModulationIntensityMin = 0;
				_sgd.ModulationIntensityMax = 0;
			} else if (_counter == 1)
			{
				
				IntensityManager.Instance.AddGazeIntensitySampleMax(_sgd.ModulationIntensityMax);
				
				Destroy(TestObjects[_counter]);
				
				_sgd.ModulationIntensityMin = IntensityManager.Instance.IntensityMin;
				_sgd.ModulationIntensityMax = IntensityManager.Instance.IntensityMax;
				_sgd.Active = false;
				_SpaceshipControls.enabled = true;
				_FireLaser.enabled = true;
				_Crosshair.enabled = true;
				EnemySpawner.SetActive(true);
			}
			
		}

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SceneManager.LoadScene("_Home");
		}
	}
}
