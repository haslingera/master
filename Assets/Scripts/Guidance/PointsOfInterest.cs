using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterest : MonoBehaviour
{

	public List<GameObject> PoiEssential = new List<GameObject>();
	
	private IPointOfInterestSelectionProcess Poisp;

	private void Start()
	{
		if (GetComponent<IPointOfInterestSelectionProcess>() != null)
		{
			Poisp = GetComponent<IPointOfInterestSelectionProcess>();
		}
	}

	private GameObject _currentPointOfInterest;

	public bool IsPointOfInterest(GameObject go)
	{
		return _currentPointOfInterest == go;
	}

	public GameObject GetCurrentPointOfInterest(PointOfInterest.PoiType type)
	{
		
		if (type == PointOfInterest.PoiType.Essential)
		{
			if (PoiEssential.Count > 0)
			{
				if (Poisp == null)
				{
					if (PoiEssential[0] != _currentPointOfInterest)
					{
						
						if (_currentPointOfInterest)
						{
							DataRecorderNew.Instance.AddNewDataSet(Time.time, _currentPointOfInterest, DataRecorderNew.Action.UnmarkedAsPOI);
						}
						
						_currentPointOfInterest = PoiEssential[0];
						
						DataRecorderNew.Instance.AddNewDataSet(Time.time, _currentPointOfInterest, DataRecorderNew.Action.MarkedAsPOI);
					}
						
					return PoiEssential[0];
				}
				
				_currentPointOfInterest = Poisp.GetPointOfInterest(this);
				return _currentPointOfInterest;
			}
			
		}

		if (_currentPointOfInterest)
		{
			DataRecorderNew.Instance.AddNewDataSet(Time.time, _currentPointOfInterest, DataRecorderNew.Action.UnmarkedAsPOI);
		}
		
		_currentPointOfInterest = null;
		return null;
	}
	
	private bool PointIsWithinFieldOfView(Vector3 point)
	{
		Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
		return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
	}

	
}
