﻿using System;
using UnityEngine;

namespace Game
{
	public class SpaceshipControls : MonoBehaviour
	{
		
		[Header("Rotation")]
		public float HorizontalTurnFactor = 2f;
		public float VerticalTurnFactor = 2f;
		
		[Header("Lean Turn")]
		public float LeanTurnFactor = 2f;
		public Transform LeanObject;
		
		[Header("Acceleration")]
		public float Acceleration = 1f;
		
		[Header("Boost")]
		public float Boost = 1f;
		
		[Header("Smoothing")]
		public float AccelerationSmoothing = 0.5f;
		public float RotationSmoothing = 0.5f;
		public float LeanSmoothing = 0.5f;

		[Header("Sound")]
		public AudioSource ThrusterAudio;
		
		private float _screenWidth;
		private float _screenHeight;
		private Vector3 _targetPosition;
		private Quaternion _targetRotation;
		private Quaternion _targetRotationLean;
		private Vector3 _centeredNormalizedMousePosition;
		private Vector3 _lastPosition;
		private Vector3 _currentPosition;
		private float _maxThrusterVolume;
		private float _desiredThrusterVolume;
	
		void Start()
		{
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			
			_targetPosition = transform.position;
			_targetRotation = transform.rotation;
			_targetRotationLean = LeanObject.rotation;

			_lastPosition = transform.position;

			_maxThrusterVolume = ThrusterAudio.volume;
		}

		void FixedUpdate()
		{
			
			CenterSquaredAndNormalizeMousePosition();
			
			_targetRotation *= Quaternion.AngleAxis(-_centeredNormalizedMousePosition.y * VerticalTurnFactor, Vector3.right);
			_targetRotation *= Quaternion.AngleAxis(_centeredNormalizedMousePosition.x * HorizontalTurnFactor, Vector3.up);
			_targetRotationLean = Quaternion.AngleAxis(-_centeredNormalizedMousePosition.x * LeanTurnFactor * 10f, Vector3.forward);
				
			
			if (Input.GetKey(KeyCode.W))
			{

				if (!ThrusterAudio.isPlaying)
				{
					ThrusterAudio.Play();
				}
				
				_targetPosition += transform.forward * (Acceleration / 100f);
				_desiredThrusterVolume = _maxThrusterVolume;

				if (Input.GetKey(KeyCode.LeftShift))
				{
					_targetPosition += transform.forward * (Boost / 100f);
					_desiredThrusterVolume = _maxThrusterVolume + 0.2f;
				}
				
				transform.position = Vector3.Lerp(transform.position, _targetPosition, AccelerationSmoothing);
	
			}
			else
			{
				_targetRotationLean = Quaternion.AngleAxis(0, Vector3.forward);
				_desiredThrusterVolume = 0;
			}
			
			LeanObject.localRotation =  Quaternion.Lerp(LeanObject.localRotation, _targetRotationLean, LeanSmoothing);
			transform.position = Vector3.Lerp(transform.position, _targetPosition, AccelerationSmoothing);
			transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, RotationSmoothing);
			ThrusterAudio.volume = Mathf.Lerp(ThrusterAudio.volume, _desiredThrusterVolume, AccelerationSmoothing);

			_lastPosition = _currentPosition;
			_currentPosition = transform.position;

		}
		
		void CenterSquaredAndNormalizeMousePosition ()
		{
			_centeredNormalizedMousePosition.x = Mathf.Clamp(Input.mousePosition.x / _screenWidth - 0.5f, -0.5f, 0.5f);
			_centeredNormalizedMousePosition.y = Mathf.Clamp(Input.mousePosition.y / _screenHeight - 0.5f, -0.5f, 0.5f);

			if (Input.GetKey(KeyCode.W))
			{
				_centeredNormalizedMousePosition.x = _centeredNormalizedMousePosition.x < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.x, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.x, 2);
				_centeredNormalizedMousePosition.y = _centeredNormalizedMousePosition.y < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.y, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.y, 2);
			}
			else
			{
				_centeredNormalizedMousePosition.x = _centeredNormalizedMousePosition.x < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.x, 4) :(float) Math.Pow(_centeredNormalizedMousePosition.x, 4);
				_centeredNormalizedMousePosition.y = _centeredNormalizedMousePosition.y < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.y, 4) :(float) Math.Pow(_centeredNormalizedMousePosition.y, 4);
			}
		}

		public float GetSpaceShipSpeed()
		{
			return (_currentPosition - _lastPosition).magnitude * 100f;
		}

		public Vector2 GetSpaceShipTurn()
		{
			return new Vector2(Math.Abs(_centeredNormalizedMousePosition.x) * HorizontalTurnFactor, Math.Abs(_centeredNormalizedMousePosition.y) * VerticalTurnFactor);
		}

	}
}