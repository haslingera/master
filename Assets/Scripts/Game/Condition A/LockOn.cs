using UnityEngine;
using UnityEngine.UI;

public class LockOn : MonoBehaviour
{
	private GameObject _enemy;

	private float _lerpTime;
	private float _currentTime;
	private bool _lerp;
	private float _time;
	private Vector2 _initalPosition = Vector2.zero;
	private Color _initalColor = new Color(1f, 1f, 1f, 0f);

	void Start () {
		ResetLock(Vector2.zero);
	}
	
	void Update () {
		
		if (_lerp)
		{
			LockOnTarget();
		}

		if (_currentTime >= _lerpTime)
		{
			_lerp = false;
			ResetLock(Vector2.zero);
		}

		_currentTime += Time.deltaTime;
	}
	
	private void ResetLock(Vector2 screenPosition)
	{
		
		if (gameObject.name.Contains("Right"))
		{
			_initalPosition.x = screenPosition.x + 100;
		}
		
		if (gameObject.name.Contains("Left"))
		{
			_initalPosition.x = screenPosition.x - 100;
		}
		
		if (gameObject.name.Contains("Up"))
		{
			_initalPosition.y = screenPosition.y + 100;
		}
		
		if (gameObject.name.Contains("Down"))
		{
			_initalPosition.y = screenPosition.y - 100;
		}
		
		GetComponent<RectTransform>().anchoredPosition = _initalPosition;
		GetComponent<Image>().color = _initalColor;
	}

	public void SetLockTarget(GameObject enemy, float time)
	{
		GameObject.Find("Target Sound").GetComponent<AudioSource>().Play();
		Vector3 enemyScreenPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
		ResetLock(new Vector2(enemyScreenPosition.x, enemyScreenPosition.y));
		
		_enemy = enemy;
		_lerpTime = time + 0.2f;
		_lerp = true;
		_time = 0;
		_currentTime = 0;
	}

	private void LockOnTarget()
	{		
		Vector3 enemyScreenPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position);
		_time += Time.deltaTime / _lerpTime;
		
		GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, new Vector2(enemyScreenPosition.x, enemyScreenPosition.y), _time);
		GetComponent<Image>().color = Color.Lerp(_initalColor, Color.white, _time);
	}
}
