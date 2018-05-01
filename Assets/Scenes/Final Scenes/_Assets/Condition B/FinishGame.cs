using UnityEngine;

public class FinishGame : MonoBehaviour
{

	public GameObject CoinCanvas;
	public GameObject CursorCanvas;
	public GameObject ExitGameCanvas;
	public SmoothRotation _SmoothRotation1;
	public SmoothRotation _SmoothRotation2;
	
	private void OnCollisionEnter(Collision other)
	{
		CoinCanvas.SetActive(false);
		CursorCanvas.SetActive(false);
		ExitGameCanvas.SetActive(true);

		_SmoothRotation1.Rotate = true;
		_SmoothRotation2.Rotate = true;
		
		Cursor.lockState = CursorLockMode.None;
	}
	
}
