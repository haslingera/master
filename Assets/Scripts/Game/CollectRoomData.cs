using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRoomData : MonoBehaviour
{

	public PointOfInterestSelectionProcessB Poisp;

	private void OnTriggerEnter(Collider other)
	{
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeRoomEntered = Time.time;
	}
	
	private void OnTriggerExit(Collider other)
	{
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeRoomExited = Time.time;
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeSpentInRoom = DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeRoomExited - DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeRoomEntered;
			
		GameObject newGameObject = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
		newGameObject.name = gameObject.name;
		newGameObject.transform.parent = gameObject.transform.parent;
		Destroy(gameObject);
	}
	
}
