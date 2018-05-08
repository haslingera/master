using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

	public float GrabLerpFactor = 10f;
	public float HoldDistance = 0.2f;

	private bool _grab;
	private GameObject _oldHit;
	private RigidbodyConstraints _oldConst;
	private RaycastHit _hit;
	private GameObject _hitObj;

	// Update is called once per frame
	void Update()
	{

		if (!_grab)
		{

			Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;

			if (Physics.Raycast(transform.position, forward, out _hit, 5f))
			{

				if (_hit.collider.gameObject)
				{

					if (_oldHit && _hit.collider.gameObject != _oldHit)
					{
						_oldHit.GetComponent<Grabable>().isGrabbed = false;
						_oldHit.GetComponent<Renderer>().material.color = Color.white;
					}

					if (_hit.collider.gameObject.GetComponent<Grabable>() != null)
					{
						if (_hit.collider.gameObject.GetComponent<Grabable>().canBeGrabbed)
						{

							_oldHit = _hit.collider.gameObject;
							_oldHit.GetComponent<Renderer>().material.color = Color.red;

							if (Input.GetMouseButtonDown(0) && !_grab)
							{
								_oldHit.GetComponent<Grabable>().isGrabbed = true;
								_hitObj = _oldHit;
								_oldConst = _hitObj.GetComponent<Rigidbody>().constraints;
								_grab = true;
							}

						}
					}
				}
				else if (_oldHit && !_grab)
				{
					_oldHit.GetComponent<Grabable>().isGrabbed = false;
					_oldHit.GetComponent<Renderer>().material.color = Color.white;
				}
			}
			else if (_oldHit)
			{
				if (_oldHit.GetComponent<Renderer>().material.color != Color.white)
				{
					_oldHit.GetComponent<Renderer>().material.color = Color.white;
				}
			}
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				Grabbing();
			}

			if (Input.GetMouseButtonUp(0))
			{
				_grab = false;
				_hitObj.GetComponent<Rigidbody>().useGravity = true;
				_hitObj.GetComponent<Rigidbody>().constraints = _oldConst;
				_hitObj.GetComponent<Grabable>().isGrabbed = false;
			}
		}
		
	}
	
	void Grabbing()
	{
		Vector3 currentPos = _hitObj.transform.position;
		Vector3 desiredPos = transform.position + transform.forward * HoldDistance;
		Vector3 calcPos = Vector3.Lerp( currentPos, desiredPos, GrabLerpFactor * Time.deltaTime );
     
		_hitObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
		_hitObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		_hitObj.GetComponent<Rigidbody>().useGravity = false;
   		_hitObj.GetComponent<Rigidbody>().MovePosition( calcPos );
	}

}
