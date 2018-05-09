using UnityEngine;

namespace Gaze
{
	public class GazeManager
	{

		private static GazeManager _instance ;
		
		private bool _gazeAvailable;
		
		private Vector2 _gaze;
				
		public static GazeManager Instance
		{
			get { return _instance ?? (_instance = new GazeManager()); }
		}
	
		public bool GazeAvailable
		{
			set
			{
				_gazeAvailable = value;
			}
			
			get
			{
				if (!_gazeAvailable)
				{
					return false;
				}
				if (_gaze.x > Screen.width || _gaze.y > Screen.height || _gaze.x < 0 || _gaze.y < 0)
				{
					return false;
				}
				return !float.IsNaN(_gaze.x);
			}
		}
		
		public bool Saccade;

		public bool Fixation;
		
		public float DistanceToComputer = 75;

		public GazeSampleObject GazeSampleObject;
		
		public Vector2 GazeVector
		{
			set
			{
				_gaze = value;
				GazeSampleObject gpo = new GazeSampleObject(value);
				GazeSampleObject = gpo;
			}
			get {return _gaze; }
		}
	
		public Vector2 GazeVectorNormalized
		{
			get { return new Vector2(_gaze.x / Screen.width, _gaze.y / Screen.height); }
		}
	
		public Vector2 SmoothGazeVector = Vector2.zero;

	}
}
