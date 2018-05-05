using UnityEngine;

public class ItemBufferObject {
	
	public GameObject go;
	public int Layer;
	public Color CurrentColor;
	public bool MeshEnabled;
	
	public float CmPog;
	public float CmPgo;
	public Vector2 Co;

	public ItemBufferObject(GameObject go, int layer, Color color)
	{
		go.GetComponent<PointOfInterest>().Id = color;
		this.go = go;
		Layer = layer;
	}
	
	public ItemBufferObject() {}


}
