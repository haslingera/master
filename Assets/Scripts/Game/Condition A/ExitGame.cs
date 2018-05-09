using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{

	public AudioSource StartAudioSource;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.GameEnded);
			DataRecorderNew.Instance.WriteDataToCsv();
			StartAudioSource.Play();
			SceneManager.LoadScene("_Home");
		}
	}
}
