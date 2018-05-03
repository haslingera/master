using UnityEngine;

[ExecuteInEditMode]
public class PointOfInterest : MonoBehaviour
{
	
	public PointOfInterestSelectionProcessB Poisp;

	public PointsOfInterest Pois;
	
	public PoiType Type = PoiType.OnlyTracking;
	
	[HideInInspector]
	public Color Id;
	public enum PoiType
	{
		Essential, OnlyTracking
	}
	
	private GameObject _gazeGuidance;
	private bool _focus;
	private readonly Vector3 _debugScale = new Vector3(1.1f, 1.1f, 1.1f);

	private bool first = true;

	void Start()
	{
		if (Type == PoiType.OnlyTracking)
		{
			RemovePOI();
		}
		else
		{
			AddPOI();
		}
	}

	private void OnDestroy()
	{
		RemovePOI();
	}

	private void OnDisable()
	{
		RemovePOI();
	}

	public void GainedFocus()
	{
		if (!_focus)
		{

			if (Poisp)
			{
				if (first && transform.parent.name == Poisp.CurrentRoom)
				{
					first = false;
					DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).Attended = true; 
					DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).TimeAttended = Time.time;

					bool poi = Pois.IsPointOfInterest(gameObject);
					if (poi)
					{
						DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).IsPointOfInterest = poi;
					}
				}
			}
			else if (first)
			{
				first = false;
				DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).Attended = true; 
				DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).TimeAttended = Time.time;
				
				bool poi = Pois.IsPointOfInterest(gameObject);
				if (poi)
				{
					DataRecorder.Instance.GetOrCreateDataSet<IDataSet>(gameObject).IsPointOfInterest = poi;
				}
			}
			
			_focus = true;
		}
	}

	public void LostFocus()
	{
		if (_focus)
		{
			_focus = false;
		}
	}

	public void SetIDColor()
	{
		GetComponent<Renderer>().material.color = Id;
	}
	
	private void OnDrawGizmos()
	{
		if (_focus)
		{
			DebugExtension.DrawLocalCube(transform,_debugScale, new Color(25/255f, 115/255f, 232/255f));
		}
	}

	private void RemovePOI()
	{
		if (_gazeGuidance != null)
		{
			PointsOfInterest pois = _gazeGuidance.GetComponent<PointsOfInterest>();
			if (pois != null)
			{
				if (pois.PoiEssential.Contains(gameObject))
				{
					pois.PoiEssential.Remove(gameObject);
				}
			}
		}
	}

	private void AddPOI()
	{
		_gazeGuidance = GameObject.Find("Gaze Guidance");
		if (_gazeGuidance != null)
		{
			PointsOfInterest pois = _gazeGuidance.GetComponent<PointsOfInterest>();
			if (pois != null)
			{
				if (!pois.PoiEssential.Contains(gameObject))
				{
					pois.PoiEssential.Add(gameObject);
				}
			}
		}
	}
}
