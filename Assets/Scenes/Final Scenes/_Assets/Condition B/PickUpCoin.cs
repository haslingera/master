using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoin : MonoBehaviour
{

	public float ReachDistance = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit[] hits;			
			hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, ReachDistance);

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].collider.gameObject.GetComponent<Coin>() != null)
				{
					Debug.Log(hits[i].collider.gameObject.name + " found");
					Destroy(hits[i].collider.gameObject);
					TimeScoreManager.Instance.IncreaseScore();
				}
			}
		}
	}
		
}
