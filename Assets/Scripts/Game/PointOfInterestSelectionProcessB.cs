using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestSelectionProcessB : MonoBehaviour, IPointOfInterestSelectionProcess
{

	public float CheckDistance = 9.0f;
	public float CoolOff = 2f;
	public LayerMask RaycastLayers;

	private float _currenTimeCooloff;

	public GameObject GetPointOfInterest(PointsOfInterest pois)
	{

		if (_currenTimeCooloff >= CoolOff)
		{

			float smallestDistance = 200f;
			int smallestDistanceCounter = -1;
			
			for (int i = 0; i < pois.PoiEssential.Count; i++)
			{

				if (IsCandidate(pois.PoiEssential[i]))
				{
					float distance = Vector3.Distance(Camera.main.transform.position, pois.PoiEssential[i].transform.position);

					if (distance < smallestDistance)
					{
						smallestDistance = distance;
						smallestDistanceCounter = i;
					}
				}
			}

			if (smallestDistanceCounter != -1)
			{
				return pois.PoiEssential[smallestDistanceCounter];
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
	
	private void Update()
	{
		_currenTimeCooloff += Time.deltaTime;
	}
}
