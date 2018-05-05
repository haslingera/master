using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRoomData : MonoBehaviour
{

	public PointOfInterestSelectionProcessB Poisp;

	private void OnTriggerEnter(Collider other)
	{
		Poisp.CurrentRoom = name;
		DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.RoomEnter);
	}
	
	private void OnTriggerExit(Collider other)
	{
		Poisp.CurrentRoom = "";
		
		DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.RoomExit);
				
		GameObject newGameObject = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
		newGameObject.name = gameObject.name;
		newGameObject.transform.parent = gameObject.transform.parent;
		Destroy(gameObject);
	}
	
}
