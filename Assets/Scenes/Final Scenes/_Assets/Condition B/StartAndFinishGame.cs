using UnityEngine;

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
		if (StartGameCanvas.activeInHierarchy && Input.GetMouseButtonUp(0))
		{
			GameObject.Find("Door Opening Sound").GetComponent<AudioSource>().Play();
			//LeanTween.delayedCall(gameObject, 0.5f, UnPauseGame);
			UnPauseGame();
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		GameObject.Find("Exit Game Sound").GetComponent<AudioSource>().Play();
		PauseGame(true);
	}

	public void QuitGame ()
	{
		Application.Quit();
	}

	public void PauseGame(bool exit)
	{
		CoinCanvas.SetActive(false);
		CursorCanvas.SetActive(false);

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
	}

	public void UnPauseGame()
	{
		CoinCanvas.SetActive(true);
		CursorCanvas.SetActive(true);
		ExitGameCanvas.SetActive(false);
		StartGameCanvas.SetActive(false);
		
		_SmoothRotation1.Rotate = true;
		_SmoothRotation2.Rotate = true;
		_Movement.Move = true;
			
		Cursor.lockState = CursorLockMode.Locked;
	}
	
}
