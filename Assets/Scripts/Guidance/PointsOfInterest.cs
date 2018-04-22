using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterest : MonoBehaviour
{

	[HideInInspector]public List<GameObject> PoiEssential = new List<GameObject>();
	[HideInInspector]public List<GameObject> PoiNonEssential = new List<GameObject>();
	
	public enum Type
	{
		Essential, NonEssential
	}
	
	public bool IsPointOfInterest(GameObject go)
	{
		return PoiEssential.Contains(go) || PoiNonEssential.Contains(go);
	}

	public void AddPointOfInterestAtPosition(GameObject go, Type type, int position)
	{
		if (type == Type.Essential)
		{
			PoiEssential.Insert(position, go);
		}
		else
		{
			PoiNonEssential.Insert(position, go);
		}
	}

	public GameObject GetCurrentPointOfInterest(Type type)
	{
		List<GameObject> iteratePoiList;
		
		if (type == Type.Essential)
		{
			iteratePoiList = PoiEssential;
		}
		else
		{
			iteratePoiList = PoiNonEssential;
		}
		
		if (iteratePoiList.Count > 0)
		{
			return iteratePoiList[0];
		}

		return null;
	}

	
}
