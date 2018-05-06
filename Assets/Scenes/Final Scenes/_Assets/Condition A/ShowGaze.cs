using Gaze;
using UnityEngine;
using UnityEngine.UI;

public class ShowGaze : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (GazeManager.Instance.GazeAvailable)
		{
			if (!GetComponent<Image>().enabled)
			{
				GetComponent<Image>().enabled = true;
			}
			
			GetComponent<RectTransform>().localPosition = GazeManager.Instance.SmoothGazeVector - new Vector2(Screen.width  * 0.5f, Screen.height * 0.5f);
		}
		else
		{
			GetComponent<Image>().enabled = false;
		}
		
	}
}
