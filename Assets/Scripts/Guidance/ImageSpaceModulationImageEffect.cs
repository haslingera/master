using UnityEngine;

namespace Guidance
{
	[ExecuteInEditMode]
	public class ImageSpaceModulationImageEffect : MonoBehaviour
	{	
		public Material ImageSpaceModulationMaterial;

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (ImageSpaceModulationMaterial)
			{
				Graphics.Blit(source,destination,ImageSpaceModulationMaterial);
			}
		}
	}
}
