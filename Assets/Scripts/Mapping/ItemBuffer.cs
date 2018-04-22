using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gaze
{
	public class ItemBuffer : MonoBehaviour
	{

		[Header("General Settings")]
		public bool ShowItemBuffer;
		public Shader ItemBufferShader;
		[Range(0, 1)]
		public float RayCastRadius = 4;
		
		public readonly Dictionary<GameObject,ItemBufferObject> ItemBufferObjects = new Dictionary<GameObject,ItemBufferObject>();
		private readonly Dictionary<GameObject,Color> ItemBufferObjectColors = new Dictionary<GameObject,Color>();
		
		private ArrayList _toRemove = new ArrayList();

		void Start()
		{				
			GetComponent<Camera>().SetReplacementShader(ItemBufferShader, "RenderType");
		}

		void Update()
		{
			if (ShowItemBuffer && GetComponent<Camera>().depth < Camera.main.depth)
			{
				GetComponent<Camera>().depth = Camera.main.depth + 1;
			} else if (!ShowItemBuffer && GetComponent<Camera>().depth > Camera.main.depth)
			{
				GetComponent<Camera>().depth = Camera.main.depth - 1;
			}

			GetItemBufferObjects();
		}

		void GetItemBufferObjects()
		{

			Dictionary<GameObject, ItemBufferObject> objects = RayCastBufferObjects();
			
			_toRemove.Clear();
							
			foreach (var obj in ItemBufferObjects)
			{
				if (!objects.ContainsKey(obj.Key))
				{
					_toRemove.Add(obj.Key);
				}
			}
			
			foreach (var obj in objects)
			{
				if (!ItemBufferObjects.ContainsKey(obj.Key))
				{
					AddToItemBuffer(obj.Key);
				}
			}

			foreach (var obj in _toRemove)
			{
				RemoveFromItemBuffer((GameObject) obj);
				((GameObject) obj).GetComponent<PointOfInterest>().LostFocus();
			}

		}

		Dictionary<GameObject, ItemBufferObject> GetAllItemBufferObjects()
		{
			Dictionary<GameObject, ItemBufferObject> objects = new Dictionary<GameObject, ItemBufferObject>();
			
			PointOfInterest[] trackables = FindObjectsOfType(typeof(PointOfInterest)) as PointOfInterest[];

			foreach (var trackable in trackables)
			{
				objects.Add(trackable.transform.gameObject, new ItemBufferObject());
			}

			return objects;
		}

		Dictionary<GameObject, ItemBufferObject> RayCastBufferObjects()
		{		
			Dictionary<GameObject, ItemBufferObject> objects = new Dictionary<GameObject, ItemBufferObject>();
			
			if (!GazeManager.Instance.GazeAvailable)
			{
				return objects;
			}

			RaycastHit [] hitsCenter = Physics.BoxCastAll(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane)), new Vector3(RayCastRadius,RayCastRadius,RayCastRadius), Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.farClipPlane)));
						
			for (int i = 0; i < hitsCenter.Length; i++)
			{
				if (hitsCenter[i].transform.gameObject.GetComponent<PointOfInterest>() &&
				    hitsCenter[i].transform.gameObject.GetComponent<PointOfInterest>().isActiveAndEnabled)
				{
					objects.Add(hitsCenter[i].transform.gameObject, new ItemBufferObject());
				}
			}

			return objects;
		}

		private void OnPreCull()
		{
			foreach(KeyValuePair<GameObject,ItemBufferObject> bufferObj in ItemBufferObjects)
			{
				bufferObj.Key.layer = LayerMask.NameToLayer("ItemBuffer");
				bufferObj.Value.CurrentColor = bufferObj.Key.GetComponent<Renderer>().material.color;
				bufferObj.Key.GetComponent<PointOfInterest>().SetIDColor();
			}
		}

		void OnPostRender()
		{	
			foreach(KeyValuePair<GameObject,ItemBufferObject> bufferObj in ItemBufferObjects)
			{
				bufferObj.Key.layer = bufferObj.Value.Layer;
				bufferObj.Key.GetComponent<Renderer>().material.color = bufferObj.Value.CurrentColor;
			}
		}

		public void AddToItemBuffer(GameObject obj)
		{
			Color id = Random.ColorHSV();

			while (ItemBufferObjectColors.ContainsValue(id))
			{
				id = Random.ColorHSV();
			}
	
			ItemBufferObjects.Add(obj, new ItemBufferObject(obj, obj.layer, id));
			ItemBufferObjectColors.Add(obj, id);
		}
		
		public void RemoveFromItemBuffer(GameObject obj)
		{
			ItemBufferObjects.Remove(obj);
			ItemBufferObjectColors.Remove(obj);
		}
	}
}	