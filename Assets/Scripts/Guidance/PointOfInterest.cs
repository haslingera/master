using UnityEngine;

[ExecuteInEditMode]
public class PointOfInterest : MonoBehaviour
{
	
	public PoiType Type = PoiType.OnlyTracking;
	
	[HideInInspector]
	public Color Id;
	public enum PoiType
	{
		Essential, NonEssential, OnlyTracking
	}
	
	private GameObject _gazeGuidance;
	private bool _focus;
	private readonly Vector3 _debugScale = new Vector3(1.1f, 1.1f, 1.1f);

	private void OnValidate()
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

	private void OnDisable()
	{
		RemovePOI();
	}

	public void GainedFocus()
	{
		if (!_focus)
		{
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
				} else if ( pois.PoiNonEssential.Contains(gameObject))
				{
					pois.PoiNonEssential.Remove(gameObject);
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
				bool essentialContain = pois.PoiEssential.Contains(gameObject);
				bool nonEssentialContain = pois.PoiNonEssential.Contains(gameObject);
				
				if (!essentialContain && !nonEssentialContain)
				{
					if (Type == PoiType.Essential)
					{
						pois.PoiEssential.Add(gameObject);
					}
					else
					{
						pois.PoiNonEssential.Add(gameObject);
					}
				} else if (essentialContain && Type == PoiType.NonEssential)
				{
					pois.PoiEssential.Remove(gameObject);
					pois.PoiNonEssential.Add(gameObject);
				} else if (nonEssentialContain && Type == PoiType.Essential)
				{
					pois.PoiEssential.Add(gameObject);
					pois.PoiNonEssential.Remove(gameObject);
				}
				
			}
		}
	}
}
