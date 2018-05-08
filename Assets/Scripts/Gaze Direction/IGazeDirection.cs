using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGazeDirection  {

	float PerceptualSpanPixel { get; }
	float ModulationRadiusPixel { get; }
}
