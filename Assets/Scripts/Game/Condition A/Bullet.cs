using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float _speed = 10f;
	private bool _enabled;
	private Vector3 _targetPosition;

	[HideInInspector] public float BulletBirthTime;

	private void Start()
	{
		BulletBirthTime = Time.time;
	}

	void Update () {
		
		if (_enabled)
		{
			transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
		}
		
		if (Vector3.Distance(transform.position, _targetPosition) < 1)
		{
			Destroy(gameObject);
		}
	}

	public void SetTargetPosition(Vector3 targetPosition, float speed)
	{
		_speed = speed;
		_targetPosition = targetPosition;
		_enabled = true;
	}

	private void OnCollisionEnter(Collision other)
	{		
		if(other.gameObject.GetComponent<Target>()){
			other.gameObject.GetComponent<Target>().TargetShot(this);
			Destroy(gameObject);
		}
	}
}
