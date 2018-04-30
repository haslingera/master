using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour {
	
	private void OnCollisionEnter(Collision other)
	{
		Debug.Log("Exit Game");
	}
	
}
