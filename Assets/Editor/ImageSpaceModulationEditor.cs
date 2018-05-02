/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImageSpaceModulation))]
public class ImageSpaceModulationEditor : Editor
{
	private GUILayoutOption[] layoutOptions;

	public override void OnInspectorGUI()
	{
						
		DrawDefaultInspector();
		
		serializedObject.Update();
		
		ImageSpaceModulation ism = target as ImageSpaceModulation;

		if (ism.ModulationType == ImageSpaceModulation.ModulationTypeModeEnum.Color)
		{
			ism.Color1 = EditorGUILayout.ColorField("Color 1", ism.Color1, null);
			if (ism.AlternateModulation)
			{
				ism.Color2 = EditorGUILayout.ColorField("Color 2", ism.Color2, null);
			}
		} else if (ism.ModulationType == ImageSpaceModulation.ModulationTypeModeEnum.Brightness)
		{
			ism.Brightness1 = EditorGUILayout.FloatField("Brightness 1", ism.Brightness1, layoutOptions);
			if (ism.AlternateModulation)
			{
				ism.Brightness2 = EditorGUILayout.FloatField("Brightness 2", ism.Brightness2, layoutOptions);
			}
		} else if (ism.ModulationType == ImageSpaceModulation.ModulationTypeModeEnum.Contrast)
		{
			ism.Contrast1 = EditorGUILayout.Slider("Contrast 1", ism.Contrast1, 0, 2, layoutOptions);
			if (ism.AlternateModulation)
			{
				ism.Contrast2 = EditorGUILayout.Slider("Contrast 2", ism.Contrast2, 0, 2, layoutOptions);
			}
		} else if (ism.ModulationType == ImageSpaceModulation.ModulationTypeModeEnum.Saturation)
		{
			ism.Saturation1 = EditorGUILayout.Slider("Contrast 1", ism.Saturation1, -1, 1, layoutOptions);
			if (ism.AlternateModulation)
			{
				ism.Saturation2 = EditorGUILayout.Slider("Contrast 2", ism.Saturation2, -1, 1, layoutOptions);
			}
		} else if (ism.ModulationType == ImageSpaceModulation.ModulationTypeModeEnum.Noise)
		{
			ism.Noise = EditorGUILayout.ObjectField("Noise", ism.Noise, typeof(Texture2D), true, layoutOptions) as Texture2D;
			ism.NoiseScale = EditorGUILayout.FloatField("Noise Scale", ism.NoiseScale, layoutOptions);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		serializedObject.ApplyModifiedProperties();
		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(ism);
		}
		
	} 

}*/
