using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target;

	public float _smoothSpeed = 0.000005f;
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		Vector3 _desiredPosition = target.position;
		Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed);
		
		Quaternion _desiredRotation= target.rotation;
		Quaternion _smoothedRotation = Quaternion.Slerp(transform.rotation, _desiredRotation, _smoothSpeed);

		transform.rotation = _smoothedRotation;
		transform.position = _smoothedPosition;
		
		//transform.LookAt(target);
	}
}
