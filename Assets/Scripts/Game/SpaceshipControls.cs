using System;
using UnityEngine;

namespace Game
{
	public class SpaceshipControls : MonoBehaviour
	{
		
		public float horizontalFactor = 2f;
		public float verticalFactor = 2f;
		
		private float _screenWidth;
		private float _screenHeight;
		private float _screenRatio;
		private Vector3 _centeredNormalizedMousePosition;
		private Rigidbody _rigidbody;
	
		void Start()
		{
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			_screenRatio =_screenWidth / _screenHeight;
			_rigidbody = transform.GetComponent<Rigidbody>();
			_rigidbody.maxAngularVelocity = 1f;
		}

		void FixedUpdate()
		{

			if (Input.GetKey(KeyCode.W))
			{
				CenterSquaredAndNormalizeMousePosition();
				//Quaternion rotationVector = gameObject.transform.rotation * Quaternion.Euler(-_centeredNormalizedMousePosition.y * verticalFactor, _centeredNormalizedMousePosition.x * horizontalFactor, 0);
				//gameObject.transform.rotation *= Quaternion.Euler(-_centeredNormalizedMousePosition.y * verticalFactor, _centeredNormalizedMousePosition.x * horizontalFactor, 0);
				_rigidbody.AddTorque(new Vector3(-_centeredNormalizedMousePosition.y * verticalFactor, _centeredNormalizedMousePosition.x * horizontalFactor, 0f));
				_rigidbody.AddForce(transform.forward * 3);
			}
			
		}
		
		void CenterSquaredAndNormalizeMousePosition ()
		{
			Debug.Log(_screenRatio);
			
			_centeredNormalizedMousePosition.x = Mathf.Clamp((Input.mousePosition.x / _screenWidth - 0.5f) * _screenRatio, -0.5f, 0.5f);
			_centeredNormalizedMousePosition.y = Mathf.Clamp(Input.mousePosition.y / _screenHeight - 0.5f, -0.5f, 0.5f);
			
			_centeredNormalizedMousePosition.x = _centeredNormalizedMousePosition.x < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.x, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.x, 2);
			_centeredNormalizedMousePosition.y = _centeredNormalizedMousePosition.y < 0 ? -(float) Math.Pow(_centeredNormalizedMousePosition.y, 2) :(float) Math.Pow(_centeredNormalizedMousePosition.y, 2);
		}

	}
}
