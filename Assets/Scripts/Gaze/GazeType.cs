using System;
using Gaze;
using UnityEngine;

public class GazeType : MonoBehaviour {
	
	[Header("Gaze Type")]
	public GazeTypes Type = GazeTypes.Temporal;
	
	[Header("Temporal Settings")]
	public float MinimumSaccadeTime = 300;

	private bool _saccade;
	private SlidingBuffer<GazeSampleObject> _slidingBuffer;
	public enum GazeTypes { Raw, Temporal }
	
	private void Start()
	{
		_slidingBuffer = new SlidingBuffer<GazeSampleObject>(2);
	}

	private void Update()
	{
		if (GazeManager.Instance.GazeAvailable)
		{
			
			_slidingBuffer.Push(GazeManager.Instance.GazeSampleObject);

			if (Type == GazeTypes.Temporal)
			{
				_saccade = IsAboveMinimumSaccadeTime();
			}

			if (Type == GazeTypes.Raw)
			{
				_saccade = true;
			}

			GazeManager.Instance.Fixation = !_saccade;
			GazeManager.Instance.Saccade = _saccade;
		}
	}

	private bool IsAboveMinimumSaccadeTime()
	{
		int count = 0;

		GazeSampleObject gpo1 = null;
		GazeSampleObject gpo2 = null;
			
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

	private float SpeedInDegreesPerSecond(GazeSampleObject gpo1, GazeSampleObject gpo2)
	{
		double degrees = RadiansToDegrees(Math.Atan((gpo1.GazePoint - gpo2.GazePoint).magnitude / Screen.dpi / (GazeManager.Instance.DistanceToComputer / 2.54f)));
		float time = (gpo2.DateTime - gpo1.DateTime).Milliseconds / 1000f;
		return (float) (degrees / time);
	}

	private double RadiansToDegrees(double radians)
	{
		return radians * (180.0 / Math.PI);
	}
}
