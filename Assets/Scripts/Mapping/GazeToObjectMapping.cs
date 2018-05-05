﻿using System.Collections.Generic;
using System.Linq;
using Gaze;
using Tobii.Gaming;
using UnityEngine;

public class GazeToObjectMapping : MonoBehaviour
{
	public GameObject _currentFocus;
	
	public LayerMask Prefilter;
	public LayerMask CollisionMask;

	private GameObject _oldFocus;
	
	public float Size = 1f;
	private bool canSee;
	
	private void Update()
	{

		_currentFocus = CalculateCurrentFixation();
			
		if (_currentFocus != _oldFocus)
		{
			if (_oldFocus)
			{
				_oldFocus.GetComponent<PointOfInterest>().LostFocus();
				_oldFocus = null;
			}

			if (_currentFocus)
			{
				_currentFocus.GetComponent<PointOfInterest>().GainedFocus();
				_oldFocus = _currentFocus;
			}
		}
		
		/*if (GazeManager.Instance.MouseAsGaze)
		{
			RaycastHit hitPoint;
		
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(GazeManager.Instance.SmoothGazeVector.x, GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane));

			if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
			{
				Focus = hitPoint.collider.gameObject;
			
				if (_currentFocus != hitPoint.collider.gameObject && hitPoint.collider.gameObject.GetComponent<PointOfInterest>() != null)
				{
					if (_currentFocus)
					{
						_currentFocus.GetComponent<PointOfInterest>().LostFocus();
					}
				
					_currentFocus = hitPoint.collider.gameObject;
					hitPoint.collider.gameObject.GetComponent<PointOfInterest>().GainedFocus();
				}

				if (_currentFocus && hitPoint.collider.gameObject.GetComponent<PointOfInterest>() == null)
				{
					if (_currentFocus.GetComponent<PointOfInterest>() != null)
					{
						_currentFocus.GetComponent<PointOfInterest>().LostFocus();
					}
					
					_currentFocus = null;
				}
				
				
			} else if(_currentFocus != null)
			{
				_currentFocus.GetComponent<PointOfInterest>().LostFocus();
				_currentFocus = null;
			}
		}
		else
		{
			_currentFocus = TobiiAPI.GetFocusedObject();

			if (_currentFocus != _oldFocus)
			{
				if (_oldFocus)
				{
					_oldFocus.GetComponent<PointOfInterest>().LostFocus();
				}

				if (_currentFocus)
				{
					_currentFocus.GetComponent<PointOfInterest>().GainedFocus();
					_oldFocus = _currentFocus;
				}
			}
		}*/
	}

	private GameObject CalculateCurrentFixation()
	{
		RaycastHit[] hitsCenter = Physics.BoxCastAll(
			Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x,
				GazeManager.Instance.SmoothGazeVector.y, Camera.main.nearClipPlane)), new Vector3(Size, Size, Size),
			Camera.main.ScreenToWorldPoint(new Vector3(GazeManager.Instance.SmoothGazeVector.x,
				GazeManager.Instance.SmoothGazeVector.y, Camera.main.farClipPlane)), Quaternion.identity, 500f, Prefilter);

		Mesh mesh;
		Vector3[] vertices;

		if (hitsCenter.Length == 0)
		{
			_currentFocus = null;
		}

		for (int i = 0; i < hitsCenter.Length; i++)
		{

			mesh = hitsCenter[i].collider.gameObject.GetComponent<MeshFilter>().mesh;
			vertices = mesh.vertices;
			canSee = false;

			for (int j = 0; j < vertices.Length; j++)
			{

				Debug.DrawRay(Camera.main.transform.position,
					hitsCenter[i].collider.gameObject.transform.TransformPoint(vertices[j]) - Camera.main.transform.position);

				RaycastHit hit;
				if (Physics.Raycast(
					new Ray(Camera.main.transform.position,
						hitsCenter[i].collider.gameObject.transform.TransformPoint(vertices[j]) - Camera.main.transform.position),
					out hit, 500f, CollisionMask))
				{
					if (hit.collider.gameObject == hitsCenter[i].collider.gameObject)
					{
						canSee = true;
						break;
					}
				}

			}

			if (canSee && GazeManager.Instance.Fixation)
			{
				return hitsCenter[i].collider.gameObject;
			}

		}
		
		return null;
	}

}
