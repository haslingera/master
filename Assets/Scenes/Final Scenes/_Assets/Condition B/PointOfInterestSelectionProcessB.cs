using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestSelectionProcessB : MonoBehaviour, IPointOfInterestSelectionProcess
{

	public string CurrentRoom;
	public float CheckDistance = 9.0f;
	public LayerMask RaycastLayers;

	public GameObject OldPointOfInterest;

	public GameObject GetPointOfInterest(PointsOfInterest pois)
	{

		if (OldPointOfInterest != null && pois.PoiEssential.Contains(OldPointOfInterest))
		{
			if (IsCandidate(OldPointOfInterest)) return OldPointOfInterest;
		}

		for (int i = 0; i < pois.PoiEssential.Count; i++)
		{

			if (IsCandidate(pois.PoiEssential[i]))
			{
				OldPointOfInterest = pois.PoiEssential[i];
				return pois.PoiEssential[i];
			}

		}

		return null;

	}

	private bool IsCandidate(GameObject toTest)
	{

		if (Vector3.Distance(Camera.main.transform.position, toTest.transform.position) < CheckDistance)
		{
			if (PointIsWithinFieldOfView(toTest.transform.position))
			{
				RaycastHit hit;
				
				if (Physics.Linecast(Camera.main.transform.position, toTest.transform.position, out hit, RaycastLayers))
				{
					Debug.Log(hit.transform.name);
					return false;
				}

				return true;
				
			}
		}
		
		return false;
	}
	
	private bool PointIsWithinFieldOfView(Vector3 point)
	{
		Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
		return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
	}
}
