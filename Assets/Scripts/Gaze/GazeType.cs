using System;
using Gaze;
using UnityEngine;

public class GazeType : MonoBehaviour {
	
	[Header("Gaze Type")]
	public Types Type = Types.Spatial;
	
	[Header("Temporal Settings")]
	public float MinimumSaccadeTime = 300;

	private bool _saccade;
	private SlidingBuffer<GazePointObject> _slidingBuffer;
	public enum Types { Raw, Spatial, Temporal }
	
	[Header("Spatial Settings")]
	[Tooltip("Tolerance radius for fixation in pixels")]
	public float FixationRadius = 50;
	
	[Header("Debug Settings")]
	public bool Debug;
	public DebugTypes DebugType = DebugTypes.Fixation;
	public enum DebugTypes { Fixation, Saccade}
	

	public bool Saccade
	{
		get
		{
			if (Debug)
			{
				return DebugType != DebugTypes.Fixation;
			}
			return _saccade;
		}
	}

	public bool Fixation
	{
		get
		{
			if (Debug)
			{
				return DebugType == DebugTypes.Fixation;
			}

			return !_saccade;
		}
	}
	
	private void Start()
	{
		_slidingBuffer = new SlidingBuffer<GazePointObject>(2);
	}

	private void Update()
	{
		
		if (GazeManager.Instance.GazeAvailable)
		{
			
			_slidingBuffer.Push(GazeManager.Instance.GazePointObject);
			
			if (Type == Types.Spatial)
			{
				_saccade = !IsWithinRadius(GazeManager.Instance.GazePointObject.GazePoint);
			}

			if (Type == Types.Temporal)
			{
				_saccade = IsAboveMinimumSaccadeTime();
			}

			if (Type == Types.Raw)
			{
				_saccade = true;
			}

			if (Input.GetKeyUp(KeyCode.F) && Debug)
			{
				if (DebugType == DebugTypes.Fixation)
				{
					DebugType = DebugTypes.Saccade;
				}
				else
				{
					DebugType = DebugTypes.Fixation;
				}
			}

			GazeManager.Instance.Fixation = Fixation;
			GazeManager.Instance.Saccade = Saccade;
		}
	}

	private bool IsWithinRadius(Vector2 point)
	{
		return Vector3.Distance(GetCentroid(), point) < FixationRadius;
	}

	private bool IsAboveMinimumSaccadeTime()
	{
		int count = 0;

		GazePointObject gpo1 = null;
		GazePointObject gpo2 = null;
			
		foreach (var gaze in _slidingBuffer)
		{
			if (count == _slidingBuffer.Count - 2)
			{
				gpo1 = gaze;
			} else if (count == _slidingBuffer.Count - 1)
			{
				gpo2 = gaze;
			}
			count++;
		}

		if (gpo1 != null && gpo2 != null)
		{
			return SpeedInDegreesPerSecond(gpo1, gpo2) > MinimumSaccadeTime;
		}
	
		return true;
	}
	

	private float SpeedInDegreesPerSecond(GazePointObject gpo1, GazePointObject gpo2)
	{
		double degrees = RadiansToDegrees(Math.Atan((gpo1.GazePoint - gpo2.GazePoint).magnitude / Screen.dpi / (GazeManager.Instance.DistanceToComputer / 2.54f)));
		float time = (gpo2.DateTime - gpo1.DateTime).Milliseconds / 1000f;
		return (float) (degrees / time);
	}

	private Vector2 GetCentroid()
	{
		Vector2 centroid = Vector2.zero;
	
		foreach (var gaze in _slidingBuffer)
		{
			centroid += gaze.GazePoint;
		}
		if (_slidingBuffer.Count == 0)
		{
			return centroid;
		}
		return centroid / _slidingBuffer.Count;
	}

	private double RadiansToDegrees(double radians)
	{
		return radians * (180.0 / Math.PI);
	}
}
