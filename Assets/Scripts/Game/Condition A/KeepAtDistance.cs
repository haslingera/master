using UnityEngine;

public class KeepAtDistance : MonoBehaviour
{

	public Transform Target;

	private float _distance;

	private void Awake()
	{
		if (Target == null && GameObject.Find("Spaceship"))
		{
			Target = GameObject.Find("Spaceship").transform;
		}
			
		_distance = Vector3.Distance(Target.position, transform.position);
	}
	
	void Update ()
	{
		transform.position = (transform.position - Target.transform.position).normalized * _distance + Target.position;
	}
}
