using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	
	private void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 1;
	}

	public void AC()
	{
		SceneManager.LoadScene("CalibrationA");
	}
	
	public void A1()
	{
		SceneManager.LoadScene("A1");
	}
	
	public void A2()
	{
		SceneManager.LoadScene("A2");
	}
	
	public void A3()
	{
		SceneManager.LoadScene("A3");
	}
	
	public void BC()
	{
		SceneManager.LoadScene("CalibrationB");
	}
	
	public void B1()
	{
		SceneManager.LoadScene("B1");
	}
	
	public void B2()
	{
		SceneManager.LoadScene("B2");
	}
	
	public void B3()
	{
		SceneManager.LoadScene("B3");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
	
}
