using System;
using UnityEngine;

namespace Gaze
{
	public class GazeSampleObject
	{

		public readonly Vector2 GazePoint;
		public readonly DateTime DateTime;

		public GazeSampleObject(Vector2 gazePoint)
		{
			GazePoint = gazePoint;
			DateTime = DateTime.Now;
		}
	
	}
}
