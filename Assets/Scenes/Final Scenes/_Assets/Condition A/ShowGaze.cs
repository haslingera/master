using Gaze;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class ShowGaze : MonoBehaviour {
	
	void Update () {
		if (GazeManager.Instance.GazeAvailable)
		{
			if (!GetComponent<Image>().enabled)
			{
				GetComponent<Image>().enabled = true;
			}
			

			if (!float.IsNaN(GazeManager.Instance.SmoothGazeVector.x) && !float.IsNaN(GazeManager.Instance.SmoothGazeVector.y))
			{
				GetComponent<RectTransform>().anchoredPosition = GazeManager.Instance.SmoothGazeVector - new Vector2(Screen.width  * 0.5f, Screen.height * 0.5f);
			}
			
		}
		else
		{
			GetComponent<Image>().enabled = false;
		}
		
	}
}
