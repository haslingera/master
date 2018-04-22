using System;
using UnityEngine;

namespace Game
{
	public class SpaceshipControls : MonoBehaviour
	{
		
		[Header("Rotation")]
		public float HorizontalTurnFactor = 2f;
		public float VerticalTurnFactor = 2f;
		public float LeanTurnFactor = 2f;
		[Header("Acceleration")]
		public float Acceleration = 1f;
		[Header("Boost")]
		public float Boost = 1f;
		[Header("Smoothing")]
		public float AccelerationSmoothing = 0.5f;
		public float RotationSmoothing = 0.5f;
		
		private float _screenWidth;
		private float _screenHeight;
		private Vector3 _targetPosition;
		private Vector3 _targetRotation;
		private Vector3 _centeredNormalizedMousePosition;
	
		void Start()
		{
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			
			_targetPosition = transform.position;
			_targetRotation = transform.rotation.eulerAngles;
		}

		void FixedUpdate()
		{
			
			CenterSquaredAndNormalizeMousePosition();
			
			if (Input.GetKey(KeyCode.W))
			{
				_targetRotation.x += -_centeredNormalizedMousePosition.y * VerticalTurnFactor;
				_targetRotation.y += _centeredNormalizedMousePosition.x * HorizontalTurnFactor;
				_targetRotation.z = -_centeredNormalizedMousePosition.x * LeanTurnFactor;
				
				_targetPosition += transform.forward * Acceleration;
				
				if (Input.GetKey(KeyCode.LeftShift))
				{
					_targetPosition += transform.forward * Boost;
				}
			}
			
			transform.position = Vector3.Lerp(transform.position, _targetPosition, AccelerationSmoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation), RotationSmoothing);
			
		}
		
		void CenterSquaredAndNormalizeMousePosition ()
		{			
			_centeredNormalizedMousePosition.x = Mathf.Clamp(Input.mousePosition.x / _screenWidth - 0.5f, -0.5f, 0.5f);
			_centeredNormalizedMousePosition.y = Mathf.Clamp(Input.mousePosition.y / _screenHeight - 0.5f, -0.5f, 0.5f);
			
			_centeredNormalizedMousePosition.x = _centeredNormalizedMousePosition.x < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.x, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.x, 2);
			_centeredNormalizedMousePosition.y = _centeredNormalizedMousePosition.y < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.y, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.y, 2);
		}

	}
}
