using UnityEngine;

[RequireComponent(typeof(ImageSpaceModulation))]
[RequireComponent(typeof(PointsOfInterest))]
[ExecuteInEditMode]
public class GazeGuidanceManager : MonoBehaviour
{

	private ImageSpaceModulation _ism;
	private PointsOfInterest _psoi;
	
	private void OnEnable()
	{
		if (GetComponent<ImageSpaceModulation>() == null)
		{
			_ism = gameObject.AddComponent<ImageSpaceModulation>();
		}
		else
		{
			_ism = gameObject.GetComponent<ImageSpaceModulation>();
		}
		
		if (GetComponent<PointsOfInterest>() == null)
		{
			_psoi = gameObject.AddComponent<PointsOfInterest>();
		}
		else
		{
			_psoi = gameObject.GetComponent<PointsOfInterest>();
		}
	}

}
