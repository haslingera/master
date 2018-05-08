using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointOfInterestSelectionProcess
{
	GameObject GetPointOfInterest(PointsOfInterest pois);
}
