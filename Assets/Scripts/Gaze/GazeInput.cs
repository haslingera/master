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
		public bool UseMouseAsGaze;
		public bool CursorLocked;
		public bool ShowGaze;

		private SlidingBuffer<Vector2> _slidingBuffer;
		private int _countTolerance = 2;
		
		private void Start()
		{
			_slidingBuffer = new SlidingBuffer<Vector2>(BufferWindow);
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
		
			if (UseMouseAsGaze && !CursorLocked)
			{
				GazeManager.Instance.GazeAvailable = true;
				GazeManager.Instance.GazeVector = Input.mousePosition;
			} else if (UseMouseAsGaze && CursorLocked)
			{
				GazeManager.Instance.GazeAvailable = true;
				GazeManager.Instance.GazeVector = new Vector2(Screen.width/2f, Screen.height/2f);
			}
			else
			{
				GazeManager.Instance.GazeAvailable = true;
				GazeManager.Instance.GazeVector = TobiiAPI.GetGazePoint().Screen;
			}

			if (BufferWindow != _slidingBuffer.GetMaxCount())
			{
				_slidingBuffer.SetMaxCount(BufferWindow);
			}

			if (_slidingBuffer.GetMaxCount() != BufferWindow)
			{
				_slidingBuffer.SetMaxCount(BufferWindow);
			}
			
			if (GazeManager.Instance.Fixation || _slidingBuffer.Count < _countTolerance)
			{
				_slidingBuffer.Push(GazeManager.Instance.GazeSampleObject.GazePoint);
				GazeManager.Instance.SmoothGazeVector = GetCentroid();
			}
			else
			{
				_slidingBuffer.Clear();
				GazeManager.Instance.SmoothGazeVector = GazeManager.Instance.GazeSampleObject.GazePoint;
			}

		}
		
		private Vector2 GetCentroid()
		{
			Vector2 centroid = Vector2.zero;
	
			foreach (var gaze in _slidingBuffer)
			{
				centroid += gaze;
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

				if (GazeManager.Instance.GazeAvailable)
				{
					foreach (Vector2 gpo in _slidingBuffer)
					{
						DebugExtension.DrawPoint(Camera.main.ScreenToWorldPoint(new Vector3(gpo.x, gpo.y, Camera.main.nearClipPlane + 5)), new Color(25/255f, 25/255f, 25/255f),0.1f);
					}
				
					DebugExtension.DrawCircle(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 5)), Camera.main.transform.forward, Color.white, 0.8f);
					DebugExtension.DrawPoint(Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane + 5)), new Color(25/255f, 115/255f, 232/255f),0.4f);
				}
			}
		}
	
	}
}

