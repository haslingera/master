using UnityEngine;

[ExecuteInEditMode]
public class PointOfInterest : MonoBehaviour
{
	
	public PoiType Type = PoiType.Essential;
	
	[HideInInspector]
	public Color Id;
	public enum PoiType
	{
		Essential, OnlyTracking, Occluder
	}
	
	private GameObject _gazeGuidance;
	public bool HasFocus;
	private readonly Vector3 _debugScale = new Vector3(1.1f, 1.1f, 1.1f);

	private bool first = true;

	void Start()
	{
		/*if (Type == PoiType.OnlyTracking)
		{
			RemovePOI();
		}
		else */
		if (Type == PoiType.Essential)
		{
			AddPOI();
		}
	}

	private void OnDestroy()
	{	
		if (_hasFocusBuffer)
		{
			DataRecorderNew.Instance.AddNewDataSet(_originalTimeUnattended, gameObject, DataRecorderNew.Action.Unattended);
		}
		
		RemovePOI();
	}

	private void OnDisable()
	{		
		RemovePOI();
	}

	private float _buffer;
	private float _bufferMax = 0.3f;
	private bool _hasFocusBuffer;

	private float _originalTimeAttended;
	private float _originalTimeUnattended;

	private void Update()
	{

		if (HasFocus && !_hasFocusBuffer)
		{
			DataRecorderNew.Instance.AddNewDataSet(_originalTimeAttended, gameObject, DataRecorderNew.Action.Attended);
			_buffer = 0;
			_hasFocusBuffer = true;
		}
		
		if (!HasFocus && _hasFocusBuffer && _buffer >= _bufferMax)
		{
			_hasFocusBuffer = false;
			_buffer = 0;
			DataRecorderNew.Instance.AddNewDataSet(_originalTimeUnattended, gameObject, DataRecorderNew.Action.Unattended);
		}
		
		if (!HasFocus)
		{
			_buffer += Time.deltaTime;
		}
		
	}

	public void GainedFocus()
	{
		if (!HasFocus)
		{
			_originalTimeAttended = Time.time;
			HasFocus = true;
		}
	}

	public void LostFocus()
	{
		if (HasFocus)
		{	
			_originalTimeUnattended = Time.time;
			HasFocus = false;
		}
		
	}

	public void SetIDColor()
	{
		GetComponent<Renderer>().material.color = Id;
	}
	
	private void OnDrawGizmos()
	{
		if (_hasFocusBuffer)
		{
			DebugExtension.DrawLocalCube(transform,_debugScale, new Color(25/255f, 115/255f, 232/255f));
		}
	}

	private void RemovePOI()
	{

		LostFocus();
		
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
