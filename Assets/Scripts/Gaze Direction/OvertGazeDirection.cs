using System.Collections;
using System.Collections.Generic;
using Gaze;
using Guidance;
using UnityEngine;

public class OvertGazeDirection : MonoBehaviour, IGazeDirection {

	public bool Active = true;

		[Header("Modulation Parameters")]		
		public float PerceptualSpanRadius = 3.8f;
		
		[Space(10)]
		public int ModulationRate = 5;
		public float ModulationIntensityMin;
		public float ModulationIntensityMax = 1f;
		public float MinModulationSize = 0f;
		public float MaxModulationSize = 3f;
		
		private float _modulationRadius;
		private PointsOfInterest _pois;
		private ImageSpaceModulationImageEffect _ism;

		public float ModulationRadiusPixel
		{
			get { return CalculateDegreesToPixel(_modulationRadius); }
		}
		
		public float PerceptualSpanPixel
		{
			get { return CalculateDegreesToPixel(PerceptualSpanRadius); }
		}

		private void Start()
		{
			
			_pois = GetComponent<PointsOfInterest>();
			_ism = Camera.main.GetComponent<ImageSpaceModulationImageEffect>();
			_ism.ModulationRate = ModulationRate;
			_ism.Size = (ModulationRadiusPixel * 2f) / Screen.height;
			_ism.Intensity = ModulationIntensityMin;
			_ism.ModulateImageSpace = false;
			
		}

		Vector3 _pointToDisplay;

		private void Update()
		{
			if (!Active)
			{
				_ism.ModulateImageSpace = false;
				return;
			}
			
			if (ChooseGameObjectToDisplay())
			{
				_ism.ModulationPositionXYZ = _pointToDisplay;
				CalculateSizeAndIntensityModulation(_pointToDisplay);
				_ism.ModulateImageSpace = true;
			}
			else
			{
				_ism.ModulateImageSpace = false;
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
		
		float CalculateDegreesToPixel (float degrees)
		{
			float distanceToComputerSquared = GazeManager.Instance.DistanceToComputer * GazeManager.Instance.DistanceToComputer;
			float radiusCm = Mathf.Sqrt(distanceToComputerSquared + distanceToComputerSquared - 2f * distanceToComputerSquared * Mathf.Cos(degrees * Mathf.Deg2Rad));
			float diameterPx = radiusCm * Screen.dpi / 2.54f;
			return diameterPx / 2f;
		}


		private void CalculateSizeAndIntensityModulation(Vector3 pointToDisplay)
		{
			Vector3 viewportPoint = Camera.main.WorldToScreenPoint(pointToDisplay);	
			_modulationRadius = Mathf.Max(MaxModulationSize * (Mathf.Min(Vector2.Distance(new Vector2(viewportPoint.x, viewportPoint.y), GazeManager.Instance.SmoothGazeVector) / Screen.height, 1f)), MinModulationSize);			
			_ism.Size = (ModulationRadiusPixel * 2f) / Screen.height;
			_ism.Intensity = Mathf.Lerp(ModulationIntensityMin, ModulationIntensityMax, Mathf.Min((Vector2.Distance(new Vector2(viewportPoint.x, viewportPoint.y), GazeManager.Instance.SmoothGazeVector) / Screen.height), 1f));
		}
}































