using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform Target;

	public float SmoothSpeedRotation = 0.5f;
	public float SmoothSpeedPosition = 0.5f;
	
	void FixedUpdate ()
	{

		Vector3 desiredPosition = Target.position;
		Quaternion desiredRotation = Target.rotation;
		
		Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, SmoothSpeedRotation);
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeedPosition);
		
		transform.rotation = smoothedRotation;
		transform.position = smoothedPosition;
		
	}
}
