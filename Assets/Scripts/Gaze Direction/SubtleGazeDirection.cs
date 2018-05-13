using System;
using Gaze;
using UnityEngine;
using UnityEngine.UI;

namespace Guidance
{
	public class SubtleGazeDirection : MonoBehaviour, IGazeDirection
	{

		public bool Active = true;

		[Header("Angle Parameters")] 
		[Range(0, 180)]
		public float CancelAngle = 10;

		[Header("Modulation Parameters")]
		
		public float ModulationRadius = 0.76f;
		public float PerceptualSpanRadius = 3.8f;
		
		[Space(10)]
		public bool IntensityCanBeModulated = true;
		public bool ModulateAlways = false;
		public bool FullIntensity = false;
		public int ModulationRate = 10;
		public float ModulationIntensityMin;
		public float ModulationIntensityMax;
		public float ModulationIntensityStepSize = 0.005f;
		public float MaxModulationDistanceMultiplier = 2f;
		
		private PointsOfInterest _pois;
		private Vector2 _lastFixationPoint;
		private bool _lastFixationPointSet;
		private float _originalModulationRadius;
		private ImageSpaceModulationImageEffect _ism;
		private Vector3 _pointToDisplay;
		private bool _showPoi = true;

		public float ModulationRadiusPixel
		{
			get { return CalculateDegreesToPixel(ModulationRadius); }
		}
		
		public float PerceptualSpanPixel
		{
			get { return CalculateDegreesToPixel(PerceptualSpanRadius); }
		}

		private void Start()
		{

			if (IntensityManager.Instance != null)
			{
				if (!IntensityCanBeModulated)
				{
					ModulationIntensityMin = IntensityManager.Instance.IntensityMin;
					ModulationIntensityMax = IntensityManager.Instance.IntensityMax;
				}
			}

			if (FullIntensity)
			{
				ModulationIntensityMin = 1f;
				ModulationIntensityMax = 1f;
			}
			
			DataRecorderNew.Instance.AddNewDataSet(ModulationIntensityMin, gameObject, DataRecorderNew.Action.ModulationIntensity);
			DataRecorderNew.Instance.AddNewDataSet(ModulationIntensityMax, gameObject, DataRecorderNew.Action.ModulationIntensity);
			
			_pois = GetComponent<PointsOfInterest>();

			_originalModulationRadius = ModulationRadius;

			_ism = Camera.main.GetComponent<ImageSpaceModulationImageEffect>();
			_ism.ModulationRate = ModulationRate;
			_ism.Size = (ModulationRadiusPixel * 2f) / Screen.height;
			_ism.Intensity = ModulationIntensityMin;
			_ism.ModulateImageSpace = false;
			
		}

		private void Update()
		{
			if (!Active)
			{
				_ism.ModulateImageSpace = false;
				return;
			}

			if (IntensityCanBeModulated)
			{
				if (Input.GetKeyUp(KeyCode.KeypadPlus))
				{
					ModulationIntensityMin = Mathf.Min(ModulationIntensityMin + ModulationIntensityStepSize, 1f);
					ModulationIntensityMax = Mathf.Min(ModulationIntensityMax + ModulationIntensityStepSize, 1f);
				} else if (Input.GetKeyUp(KeyCode.KeypadMinus))
				{
					ModulationIntensityMin = Mathf.Max(ModulationIntensityMin - ModulationIntensityStepSize, 0f);
					ModulationIntensityMax = Mathf.Max(ModulationIntensityMax - ModulationIntensityStepSize, 0f);
				}
			}

			if (ModulateAlways)
			{
				if (ChooseGameObjectToDisplay())
				{
					_ism.ModulationPositionXYZ = _pointToDisplay;
					CalculateSizeAndIntensityModulation(_pointToDisplay);
					SetLastFixationPoint();
					_ism.ModulateImageSpace = true;
				}
				else
				{
					_ism.ModulateImageSpace = false;
				}
			}
			else
			{
				if (ChooseGameObjectToDisplay())
				{
					_ism.ModulationPositionXYZ = _pointToDisplay;
					CalculateSizeAndIntensityModulation(_pointToDisplay);
					SetLastFixationPoint();

					if (_lastFixationPointSet)
					{
						ShowLastFixationSaccadePointTriangle(_pointToDisplay);
					}
			
					_ism.ModulateImageSpace = ShowPoi(_pointToDisplay);
				}
				else
				{
					_ism.ModulateImageSpace = false;
					_showPoi = false;
				}
			}

			
			
		}

		private bool ChooseGameObjectToDisplay()
		{
			if (_pois.GetCurrentPointOfInterest(PointOfInterest.PoiType.Essential) != null)
			{
				_pointToDisplay = _pois.GetCurrentPointOfInterest(PointOfInterest.PoiType.Essential).transform.position;
				return true;
			}

			return false;
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
	
		private bool ShowPoi(Vector3 pointToDisplay)
		{
			
			float angle = CalculateAngleBetweenFixationSaccadePoi(pointToDisplay);

			if (angle > 0 && angle <= CancelAngle)
			{
				_showPoi = false;
				return _showPoi;
			}
			
			_showPoi = CalcPoiDistance(pointToDisplay);
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

		private bool CalcPoiDistance(Vector3 pointToDisplay)
		{

			if (!PointIsWithinFieldOfView(pointToDisplay)) return false;
			
			Vector2 poiPositionScreen = Camera.main.WorldToScreenPoint(pointToDisplay);

			return IsDistanceToGazeEnough(poiPositionScreen);
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
		
		float CalculateDegreesToPixel (float degrees)
		{
			float distanceToComputerSquared = GazeManager.Instance.DistanceToComputer * GazeManager.Instance.DistanceToComputer;
			float radiusCm = Mathf.Sqrt(distanceToComputerSquared + distanceToComputerSquared - 2f * distanceToComputerSquared * Mathf.Cos(degrees * Mathf.Deg2Rad));
			float diameterPx = radiusCm * Screen.dpi / 2.54f;
			return diameterPx / 2f;
		}
	
		private bool IsDistanceToGazeEnough(Vector2 pointToDisplay)
		{
			return Vector2.Distance(pointToDisplay, GazeManager.Instance.SmoothGazeVector) > ModulationRadiusPixel + PerceptualSpanPixel;
		}

		private void CalculateSizeAndIntensityModulation(Vector3 pointToDisplay)
		{
			Vector3 viewportPoint = Camera.main.WorldToScreenPoint(pointToDisplay);	
			ModulationRadius = _originalModulationRadius * (1f + (Mathf.Max(MaxModulationDistanceMultiplier - 1, 0f) * Mathf.Min(Vector2.Distance(new Vector2(viewportPoint.x, viewportPoint.y), GazeManager.Instance.SmoothGazeVector) / Screen.height, 1f)));			
			_ism.Size = (ModulationRadiusPixel * 2f) / Screen.height;
			_ism.Intensity = Mathf.Lerp(ModulationIntensityMin, ModulationIntensityMax, Mathf.Min(Vector2.Distance(new Vector2(viewportPoint.x, viewportPoint.y), GazeManager.Instance.SmoothGazeVector) / Screen.height, 1f));
		}
	
	}
}
