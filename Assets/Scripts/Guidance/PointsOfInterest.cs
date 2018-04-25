using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterest : MonoBehaviour
{

	[HideInInspector]public List<GameObject> PoiEssential = new List<GameObject>();
	[HideInInspector]public List<GameObject> PoiNonEssential = new List<GameObject>();
	
	public bool IsPointOfInterest(GameObject go)
	{
		return PoiEssential.Contains(go) || PoiNonEssential.Contains(go);
	}

	public GameObject GetCurrentPointOfInterest(PointOfInterest.PoiType type)
	{
		
		if (type == PointOfInterest.PoiType.Essential)
		{
			if (PoiEssential.Count > 0)
			{
				return PoiEssential[0];
			}
		}
		
		if (type == PointOfInterest.PoiType.NonEssential)
		{
			if (PoiNonEssential.Count > 0)
			{
				return PoiNonEssential[0];
			}
		}

		return null;
	}

	
}
