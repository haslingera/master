using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float _speed = 10f;
	private bool _enabled;
	private Vector3 _targetPosition;
	
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
	
}
