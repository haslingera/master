using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour {

	public float RotationSpeed = 80;

	public GameObject RotateAround;
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (GetComponent<Grabable>() != null && GetComponent<Grabable>().isGrabbed)
		{
			return;
		}
		
		RotateRigidBodyAroundPointBy(GetComponent<Rigidbody>(), RotateAround.transform.position, Vector3.up, RotationSpeed);
	}
	
	public void RotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
	{
		Quaternion q = Quaternion.AngleAxis(angle, axis);
		rb.MovePosition(q * (rb.transform.position - origin) + origin);
		rb.MoveRotation(rb.transform.rotation * q);
	}
}
