using Gaze;
using UnityEngine;
using UnityEngine.UI;

namespace Guidance
{
	public class SubtleGazeDirection : MonoBehaviour
	{

		public bool Active = true;

		[Header("Angle Parameters")] 
		[Range(0, 180)]
		public float CancelAngle = 10;
		
		[Header("Distance Parameters")]
		public bool AutoDistance = true;
		[Range(0, 1)]
		public float CancelDistance = 0.1f;
		[Range(0, 1)]
		public float StartDistance = 0.4f;
		
		private PointsOfInterest _pois;
		private ImageSpaceModulation _ism;

		private Vector2 _lastFixationPoint;
		private bool _lastFixationPointSet;

		private void Start()
		{
			_pois = GetComponent<PointsOfInterest>();
			_ism = GetComponent<ImageSpaceModulation>();
		}

		private void Update()
		{
			if (!Active) return;
			Vector3 pointToDisplay = ChooseGameObjectToDisplay();
			
			if (pointToDisplay == Vector3.positiveInfinity) return;
			_ism.ModulationPositionXYZ = pointToDisplay;
			SetLastFixationPoint();

			if (!_lastFixationPointSet) return;
			ShowLastFixationSaccadePointTriangle(pointToDisplay);

			_ism.ModulateImageSpace = ShowPoi(pointToDisplay);
			
		}

		private Vector3 ChooseGameObjectToDisplay()
		{
			if (_pois.GetCurrentPointOfInterest(PointsOfInterest.Type.Essential) != null) return _pois.GetCurrentPointOfInterest(PointsOfInterest.Type.Essential).transform.position;
			if (_pois.GetCurrentPointOfInterest(PointsOfInterest.Type.NonEssential) != null) return _pois.GetCurrentPointOfInterest(PointsOfInterest.Type.NonEssential).transform.position;
			return Vector3.positiveInfinity;
		}
	

		private void SetLastFixationPoint()
		{
			if (GazeManager.Instance.Fixation)
			{
				_lastFixationPoint = GazeManager.Instance.SmoothGazeVector;
				_lastFixationPointSet = true;
			}
		}

		private void ShowLastFixationSaccadePointTriangle(Vector3 poiPosition)
		{
			Vector3 lastFixationPointWorld = Camera.main.ScreenToWorldPoint(new Vector3(_lastFixationPoint.x, _lastFixationPoint.y, Camera.main.nearClipPlane + 1));
			Vector3 currentGazePointWorld = Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 1));
		
			Debug.DrawLine(lastFixationPointWorld, poiPosition, Color.green);
			Debug.DrawLine(lastFixationPointWorld, currentGazePointWorld, Color.green);
		}

		private bool _showPoi = true;
		
		private bool ShowPoi(Vector3 pointToDisplay)
		{
			
			float angle = CalculateAngleBetweenFixationSaccadePoi(pointToDisplay);

			if (angle > 0 && angle <= CancelAngle || CalcPoiDistance(pointToDisplay, CancelDistance, true))
			{
				_showPoi = false;
			}

			if (_showPoi == false)
			{
				_showPoi = CalcPoiDistance(pointToDisplay, StartDistance, false);
			}
			
			return _showPoi;
			
		}
		
		private float CalculateAngleBetweenFixationSaccadePoi(Vector3 pointToDisplay)
		{
			Vector2 poiPositionScreen = Camera.main.WorldToScreenPoint(pointToDisplay);
		
			Vector2 fixationSaccade = GazeManager.Instance.SmoothGazeVector -_lastFixationPoint;
			Vector2 fixationPoi = poiPositionScreen -_lastFixationPoint;

			float angle = AngleBetweenDirections(fixationPoi, fixationSaccade);			
			return angle;
		}

		private bool CalcPoiDistance(Vector3 pointToDisplay, float maxDistance, bool smallerThan)
		{

			if (!PointIsWithinFieldOfView(pointToDisplay)) return false;
			
			Vector2 poiPositionScreen = Camera.main.WorldToScreenPoint(pointToDisplay);
			Vector2 poiPositionScreenNormalized = new Vector2(poiPositionScreen.x / Screen.width, poiPositionScreen.y / Screen.height);
			
			float distance = Vector2.Distance(GazeManager.Instance.SmoothGazeVectorNormalized, poiPositionScreenNormalized);

			if (smallerThan) return distance < maxDistance;
			
			return distance > maxDistance;
		}
		
		

		private float AngleBetweenDirections(Vector2 vec1, Vector2 vec2)
		{
			if (vec1.magnitude <= 0 || vec2.magnitude <= 0)
			{
				return 0;
			}
		
			return Mathf.Acos(Vector2.Dot(vec1, vec2) / (vec1.magnitude * vec2.magnitude)) * Mathf.Rad2Deg;
		}

		private bool PointIsWithinFieldOfView(Vector3 point)
		{
			Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
			return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
		}
	
	}
}
