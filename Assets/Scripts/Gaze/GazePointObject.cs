using System;
using UnityEngine;

namespace Gaze
{
	public class GazePointObject
	{

		public readonly Vector2 GazePoint;
		public readonly DateTime DateTime;

		public GazePointObject(Vector2 gazePoint)
		{
			GazePoint = gazePoint;
			DateTime = DateTime.Now;
		}
	
	}
}
