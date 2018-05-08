using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoin : MonoBehaviour
{

	public AudioSource CoinPickupSound;

	public GameObject ExitGameCanvas;

	public float ReachDistance = 2f;
	
	// Update is called once per frame
	void Update () {
		
			if (!ExitGameCanvas.activeInHierarchy && Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
			
				//hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, ReachDistance);
			
				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, ReachDistance))
				{
					if (hit.collider.gameObject.GetComponent<Coin>() != null)
					{
						hit.collider.gameObject.GetComponent<CollectCoinData>().CoinCollected();
						hit.collider.gameObject.GetComponent<CollectCoinData>().DeleteCoinAfterDataCollection();
					
						CoinPickupSound.Play();
						TimeScoreManager.Instance.IncreaseScore();
					}
				}
			}
		}
		
}
