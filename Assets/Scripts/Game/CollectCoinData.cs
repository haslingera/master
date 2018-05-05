using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinData : MonoBehaviour {
	
	public void CoinCollected()
	{
		DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.Collected);
	}

	public void DeleteCoinAfterDataCollection()
	{
		Destroy(gameObject);
	}
	
}
