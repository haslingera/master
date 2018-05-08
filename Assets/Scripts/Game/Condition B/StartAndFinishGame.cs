using Guidance;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class StartAndFinishGame : MonoBehaviour
{

	public GameObject StartGameCanvas;
	public GameObject CoinCanvas;
	public GameObject CursorCanvas;
	public GameObject ExitGameCanvas;
	
	public SmoothRotation _SmoothRotation1;
	public SmoothRotation _SmoothRotation2;
	public Movement _Movement;

	private void Start()
	{
		
		if (StartGameCanvas.activeInHierarchy)
		{
			PauseGame(false);
		}
		else
		{
			UnPauseGame();
		}
	}

	void Update()
	{
		
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			DataRecorderNew.Instance.AddNewDataSet(_endTime, gameObject, DataRecorderNew.Action.GameEnded);
			DataRecorderNew.Instance.WriteDataToCsv();
			SceneManager.LoadScene("_Home");
		}
		
		if (StartGameCanvas.activeInHierarchy && Input.GetMouseButtonUp(0))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.GameStarted);
			GameObject.Find("Door Opening Sound").GetComponent<AudioSource>().Play();
			UnPauseGame();
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (GetComponent<Renderer>().isVisible)
		{
			_endTime = Time.time;
			PauseGame(true);
		}
	}

	private float _endTime;

	public void QuitGame ()
	{
		DataRecorderNew.Instance.AddNewDataSet(_endTime, gameObject, DataRecorderNew.Action.GameEnded);
		DataRecorderNew.Instance.WriteDataToCsv();
		GameObject.Find("Exit Game Sound").GetComponent<AudioSource>().Play();
		SceneManager.LoadScene("_Home");
	}

	public void PauseGame(bool exit)
	{
		CoinCanvas.SetActive(false);
		CursorCanvas.SetActive(false);

		Camera.main.GetComponent<ImageSpaceModulationImageEffect>().ModulateImageSpace = false;

		if (exit)
		{
			ExitGameCanvas.SetActive(true);
		}
		else
		{
			StartGameCanvas.SetActive(true);
		}		

		_SmoothRotation1.Rotate = false;
		_SmoothRotation2.Rotate = false;
		_Movement.Move = false;
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void UnPauseGame()
	{
		CoinCanvas.SetActive(true);
		CursorCanvas.SetActive(true);
		ExitGameCanvas.SetActive(false);
		StartGameCanvas.SetActive(false);
		
		Camera.main.GetComponent<ImageSpaceModulationImageEffect>().ModulateImageSpace = true;
		
		_SmoothRotation1.Rotate = true;
		_SmoothRotation2.Rotate = true;
		_Movement.Move = true;
			
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
}
