using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawRing : MonoBehaviour {
	[Range(0,100)]
	public int segments = 50;

	public GameObject Parent;
	
	LineRenderer line;

	void Start ()
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.positionCount = segments + 1;
	}

	private void Update()
	{
		CreatePoints();
	}

	void CreatePoints ()
	{
		float x;
		float z;

		float xradius = (transform.position - new Vector3(Parent.transform.position.x, transform.position.y,Parent.transform.position.z)).magnitude;

		float angle = 20f;

		for (int i = 0; i < segments + 1; i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * xradius;

			line.SetPosition (i,new Vector3(x,transform.position.y - transform.localScale.y / 2f,z) + Parent.transform.position);

			angle += 360f / segments;
		}
	}
}