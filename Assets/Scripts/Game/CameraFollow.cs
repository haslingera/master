using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target;

	public float SmoothSpeedRotation = 0.5f;
	public float SmoothSpeedPosition = 0.5f;
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		Vector3 desiredPosition = target.position;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeedRotation);

		Quaternion desiredRotation = target.rotation;
		Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, SmoothSpeedPosition);
		
		transform.rotation = smoothedRotation;
		transform.position = smoothedPosition;
		
		Debug.Log(target.localRotation.eulerAngles);

		/*Vector3 tmp = transform.localRotation.eulerAngles;
		tmp.z = 0;
		transform.localRotation = Quaternion.Euler(tmp);*/

		//transform.LookAt(target);
	}
}
