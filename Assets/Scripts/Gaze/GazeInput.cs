using Tobii.Gaming;
using UnityEngine;

namespace Gaze
{
	[RequireComponent(typeof(GazeType))]
	public class GazeInput : MonoBehaviour
	{	
		[Header("General Settings")]
		[Range(1, 100)]
		public int BufferWindow = 10;
		public bool UseMouseAsInput;
		public bool CursorLocked;
		public bool ShowGaze;

		private SlidingBuffer<GazePointObject> _slidingBuffer;
		private GazeType _gazeType;
		private int _countTolerance = 2;
		
		private void Start()
		{
			_slidingBuffer = new SlidingBuffer<GazePointObject>(BufferWindow);
			_gazeType = GetComponent<GazeType>();
		}

		private void Update()
		{

			if (Cursor.lockState == CursorLockMode.Locked)
			{
				CursorLocked = true;
			}
			else
			{
				CursorLocked = false;
			}
		
			if (UseMouseAsInput && !CursorLocked)
			{
				GazeManager.Instance.SetGazeAvailable();
				GazeManager.Instance.SetGazeVector(Input.mousePosition);
			} else if (UseMouseAsInput && CursorLocked)
			{
				GazeManager.Instance.SetGazeAvailable();
				GazeManager.Instance.SetGazeVector(new Vector2(Screen.width/2f, Screen.height/2f));
			}
			else
			{
				GazeManager.Instance.SetGazeAvailable();
				GazeManager.Instance.SetGazeVector(TobiiAPI.GetGazePoint().Screen);
			}

			if (BufferWindow != _slidingBuffer.GetMaxCount())
			{
				_slidingBuffer.SetMaxCount(BufferWindow);
			}

			if (_slidingBuffer.GetMaxCount() != BufferWindow)
			{
				_slidingBuffer.SetMaxCount(BufferWindow);
			}
			
			if (_gazeType.Fixation || _slidingBuffer.Count < _countTolerance)
			{
				_slidingBuffer.Push(GazeManager.Instance.GazePointObject);
				GazeManager.Instance.SetSmoothGazeVector(GetCentroid());
			}
			else
			{
				_slidingBuffer.Clear();
				GazeManager.Instance.SetSmoothGazeVector(GazeManager.Instance.GazePointObject.GazePoint);
			}

		}
		
		private Vector2 GetCentroid()
		{
			Vector2 centroid = Vector2.zero;
	
			foreach (var gaze in _slidingBuffer)
			{
				centroid += gaze.GazePoint;
			}
			if (_slidingBuffer.Count == 0)
			{
				return centroid;
			}
			return centroid / _slidingBuffer.Count;
		}
	
		private void OnDrawGizmos()
		{
			if (ShowGaze)
			{
				if (_slidingBuffer == null) return;
				
				foreach (GazePointObject gpo in _slidingBuffer)
				{
					DebugExtension.DrawPoint(Camera.main.ScreenToWorldPoint(new Vector3(gpo.GazePoint.x, gpo.GazePoint.y, Camera.main.nearClipPlane + 5)), new Color(25/255f, 25/255f, 25/255f),0.1f);
				}
				
				DebugExtension.DrawCircle(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 5)), Vector3.forward, Color.white, 0.8f);
				DebugExtension.DrawPoint(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 5)), new Color(25/255f, 115/255f, 232/255f),0.4f);
				//Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 5)), 0.05f);
			}
		}
	
	}
}

