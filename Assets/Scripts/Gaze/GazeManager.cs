using UnityEngine;

namespace Gaze
{
	public class GazeManager
	{

		private static GazeManager _instance ;

		public static GazeManager Instance
		{
			get { return _instance ?? (_instance = new GazeManager()); }
		}

		private bool _gazeAvailable;
	
		public bool GazeAvailable
		{
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
	
		private Vector2 _gaze;
	
		public Vector2 GazeVector
		{
			get {return _gaze; }
		}
	
		public Vector2 GazeVectorNormalized
		{
			get { return new Vector2(_gaze.x / Screen.width, _gaze.y / Screen.height); }
		}
	
		private GazePointObject _gazePointObject;
	
		public GazePointObject GazePointObject
		{
			get {return _gazePointObject; }
		}
	
		private Vector2 _smoothGaze;
	
		public Vector2 SmoothGazeVector
		{
			get {return _smoothGaze; }
		}
	
		public Vector2 SmoothGazeVectorNormalized
		{
			get { return new Vector2(_smoothGaze.x / Screen.width, _smoothGaze.y / Screen.height); }
		}

		public float DistanceToComputer = 50;

		public float FovealVisionDegrees = 6;
		
		public float FovealVisionRadians = 6 * Mathf.Deg2Rad;

		public void SetGazeVector(Vector2 gaze)
		{	
			_gaze = gaze;
			GazePointObject gpo = new GazePointObject(gaze);
			_gazePointObject = gpo;
		}
	
		public void SetSmoothGazeVector(Vector2 gaze)
		{
			_smoothGaze = gaze;
		}
	
		public void SetGazeAvailable()
		{
			if (!_gazeAvailable)
			{
				_gazeAvailable = true;
			}
		}

		public bool Saccade;

		public bool Fixation;

	}
}
