using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Grabable : MonoBehaviour
{
	public bool canBeGrabbed;

	public bool isGrabbed;

	public bool IsColliding { get; private set; }

	private void OnCollisionEnter(Collision collision)
	{
		IsColliding = true;
	}

	private void OnTriggerStay(Collider other)
	{
		IsColliding = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		IsColliding = false;
	}
}
