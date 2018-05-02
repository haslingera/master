using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinData : MonoBehaviour {
	
	public void CoinCollected()
	{
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).Collected = true;
		DataRecorder.Instance.GetOrCreateDataSet<DataSetConditionB>(gameObject).TimeCollected = Time.time;
	}

	public void DeleteCoinAfterDataCollection()
	{
		Destroy(gameObject);
	}
	
}
