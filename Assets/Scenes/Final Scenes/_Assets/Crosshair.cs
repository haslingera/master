using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{

	public GameObject CrosshairImage;

	// Use this for initialization
	void Start ()
	{
		//Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void Update ()
	{
		CrosshairImage.GetComponent<RectTransform>().localPosition = Input.mousePosition - new Vector3(Screen.width  * 0.5f, Screen.height * 0.5f, 0f);
	}
}
