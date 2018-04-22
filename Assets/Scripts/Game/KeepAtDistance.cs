using UnityEngine;

public class KeepAtDistance : MonoBehaviour
{

	public Transform Target;

	private float _distance;
	
	void Start ()
	{
		_distance = Vector3.Distance(Target.position, transform.position);
	}
	
	void Update ()
	{
		transform.position = (transform.position - Target.transform.position).normalized * _distance + Target.position;
	}
}
