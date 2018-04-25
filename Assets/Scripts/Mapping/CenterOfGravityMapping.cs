using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaze
{
	public class CenterOfGravityMapping : MonoBehaviour
	{

		public ComputeShader CenterOfGravityShader;
		public GameObject currentMappedObject;

		public bool TakeScreenshot;
		public bool ShowGazeMapping;
		public int ResolutionDownsize = 3;

		private RenderTexture _imageBuffer;
		private ComputeBuffer _countBuffer;
		private BufferStruct[] _countBufferRead;
		private int _kernelIdx;
		private const int KernelSize = 16;

		private const float GaussianKernelSize = 1.0f;
		private int _textureWidth;
		private int _textureHeight;		
		
		private ItemBuffer _itemBuffer;
		private readonly GUIStyle _style = new GUIStyle();
		
		struct BufferStruct
		{
			public Vector3 Id;
			public uint XCount;
			public uint YCount;
			public uint Count;
		}

		private void Start()
		{
			_textureWidth = Screen.width / ResolutionDownsize;
			_textureHeight = Screen.height / ResolutionDownsize;
			
			_imageBuffer = new RenderTexture(_textureWidth, _textureHeight, 24);
			_imageBuffer.enableRandomWrite = true;
			_imageBuffer.Create();
			
			_kernelIdx = CenterOfGravityShader.FindKernel("COGMain");

			_itemBuffer = GetComponent<ItemBuffer>();
			_style.alignment = TextAnchor.MiddleCenter;
		}

		private void Update()
		{
			if (TakeScreenshot)
			{
				TakeScreenshot = false;
				Screenshot.RenderTextureToPng(_imageBuffer, Application.dataPath);
			}

			_itemBuffer.CheckItemBuffer();
			
			if (_itemBuffer.ItemBufferObjects.Count > 0)
			{
				RunShader();
				MapCenteroids();
				MapGaze();
			}
			
			ColorGazeMapping();
			
		}

		private void MapGaze()
		{			
			if(GazeManager.Instance.GazeAvailable){
				CalculatePOgs(GazeManager.Instance.GazeVectorNormalized);
			}
		}

		public ItemBufferObject GetFixatedGameObject()
		{
			ItemBufferObject highestProbabilityObject = null;

			if (_itemBuffer.ItemBufferObjects.Count > 0)
			{
				foreach (var obj in _itemBuffer.ItemBufferObjects)
				{
					if (highestProbabilityObject == null)
					{
						highestProbabilityObject = obj.Value;
					}
					if (obj.Value.CmPog > highestProbabilityObject.CmPog)
					{
						highestProbabilityObject = obj.Value;
					}
				}
			}
			
			return highestProbabilityObject;
		}

		private void ColorGazeMapping()
		{
			
			if (ShowGazeMapping && GazeManager.Instance.GazeAvailable)
			{
				
				ItemBufferObject fixated = GetFixatedGameObject();

				if (fixated == null)
				{
					currentMappedObject = null;
					return;
				}
				
				foreach (var obj in _itemBuffer.ItemBufferObjects)
				{
					obj.Key.GetComponent<PointOfInterest>().LostFocus();
				}

				currentMappedObject = fixated.go;
				fixated.go.GetComponent<PointOfInterest>().GainedFocus();
				
			} else if (!ShowGazeMapping)
			{
				foreach (var obj in _itemBuffer.ItemBufferObjects)
				{
					obj.Value.go.GetComponent<PointOfInterest>().LostFocus();
				}
			}
		}
		
		void RunShader()
		{
			if (_itemBuffer.ItemBufferObjects.Count == 0) return;
			
			ReleaseBuffers();
			InitBuffers();
				
			CenterOfGravityShader.SetTexture(_kernelIdx, "Source", _imageBuffer);
			CenterOfGravityShader.SetBuffer(_kernelIdx, "Bss", _countBuffer);
			CenterOfGravityShader.Dispatch(_kernelIdx, _imageBuffer.width/KernelSize + (KernelSize - 1), _imageBuffer.height/KernelSize + (KernelSize - 1), 1);
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, _imageBuffer);
			Graphics.Blit(_imageBuffer, dest);
		}

		private void ReleaseBuffers()
		{
			if (_countBuffer != null)
			{
				_countBuffer.Release();
				_countBuffer.Dispose();
			}
		}

		private void InitBuffers()
		{
			BufferStruct [] bss = new BufferStruct[_itemBuffer.ItemBufferObjects.Count];
			int count = 0;

			foreach (var obj in _itemBuffer.ItemBufferObjects)
			{
				Color id = obj.Key.GetComponent<PointOfInterest>().Id;
				bss[count].Id = new Vector3((float)Math.Round(id.r, 5), (float)Math.Round(id.g, 5), (float)Math.Round(id.b, 5));
				bss[count].XCount = 0;
				bss[count].YCount = 0;
				bss[count].Count = 0;
				count++;
			}
			
			_countBuffer = new ComputeBuffer(_itemBuffer.ItemBufferObjects.Count, 24);
			_countBuffer.SetData(bss);
			_countBufferRead = new BufferStruct[_itemBuffer.ItemBufferObjects.Count];
		}

		private void  MapCenteroids()
		{
			if (_countBufferRead == null) return;
			
			_countBuffer.GetData(_countBufferRead);
			
			for (int i = 0; i < _countBufferRead.Length; i++)
			{					
				foreach (var obj in _itemBuffer.ItemBufferObjects)
				{
					Color id = obj.Key.GetComponent<PointOfInterest>().Id;
					if (id.r > _countBufferRead[i].Id.x - 0.01 && id.r < _countBufferRead[i].Id.x + 0.01 && id.g > _countBufferRead[i].Id.y - 0.01 && id.g < _countBufferRead[i].Id.y + 0.01 && id.b > _countBufferRead[i].Id.z - 0.01 && id.b < _countBufferRead[i].Id.z + 0.01)
					{
						if (_countBufferRead[i].Count != 0)
						{
							obj.Value.Co = new Vector2(_countBufferRead[i].XCount / (float) _countBufferRead[i].Count / _textureWidth, _countBufferRead[i].YCount / (float) _countBufferRead[i].Count / _textureHeight);
						}
					}
				}
			}
		}
		
		private void OnDestroy()
		{
			if (_countBuffer != null)
			{
				_countBuffer.Dispose();
			}
		}

		private void CalculatePOgs(Vector2 gaze)
		{
			float denominator = 0;
			float po = 1.0f / _itemBuffer.ItemBufferObjects.Count;
			
			foreach (var obj in _itemBuffer.ItemBufferObjects)
			{
				obj.Value.CmPgo = CalculatePgO(gaze, obj.Value);
				denominator += obj.Value.CmPgo * po;
			}
			
			foreach (var obj in _itemBuffer.ItemBufferObjects)
			{
				obj.Value.CmPog = obj.Value.CmPgo * po / denominator;
			}
						
		}

		private float CalculatePgO(Vector2 gaze, ItemBufferObject ibo)
		{
			double divisor = Math.Exp(-((ibo.Co - gaze).magnitude * (ibo.Co - gaze).magnitude / (2 * (GaussianKernelSize * GaussianKernelSize))));
			double denominator = 1.0 / (2.0 * Math.PI * (GaussianKernelSize * GaussianKernelSize));
			return (float) (divisor * denominator);
		}
		
		private void OnDrawGizmos()
		{
			if (_itemBuffer != null)
			{
				foreach(KeyValuePair<GameObject,ItemBufferObject> bufferObj in _itemBuffer.ItemBufferObjects)
				{
					if (bufferObj.Value.Co.x > 0 && bufferObj.Value.Co.y > 0)
					DebugExtension.DebugPoint(Camera.main.ScreenToWorldPoint(new Vector3(bufferObj.Value.Co.x * Screen.width, bufferObj.Value.Co.y * Screen.height, Camera.main.nearClipPlane + 1)), Color.cyan, 0.2f);
					//Handles.Label(bufferObj.Key.transform.position, "P(O|g) = " + bufferObj.Value.CmPog, _style);
				}
			}
		}
	}
}