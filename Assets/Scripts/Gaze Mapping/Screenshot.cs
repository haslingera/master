using System.IO;
using UnityEngine;

public class Screenshot {
	public static void RenderTextureToPng(RenderTexture rt, string pngOutPath)
	{
		var oldRt = RenderTexture.active;
		var tex = new Texture2D(rt.width, rt.height);
		RenderTexture.active = rt;
		tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		tex.Apply();
		File.WriteAllBytes(pngOutPath + "/Screenshots/itemBuffer" + Random.Range(0f, 1000f) + ".png", tex.EncodeToPNG());
		RenderTexture.active = oldRt;
	}
}
