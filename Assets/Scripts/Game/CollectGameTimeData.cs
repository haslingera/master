using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGameTimeData : MonoBehaviour {
	
	private void OnApplicationQuit()
	{
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimePlayedTotal = Time.time;
	}
	
}
