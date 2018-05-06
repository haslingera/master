using UnityEngine;

public class FireLaser : MonoBehaviour
{

	public GameObject Bullet;
	public AudioSource BulletAudio;
	
	public GameObject MuzzleLeft;
	public Light MuzzleLightLeft;
	public ParticleSystem MuzzleFlashLeft;
	
	public GameObject MuzzleRight;
	public Light MuzzleLightRight;
	public ParticleSystem MuzzleFlashRight;

	public float LaserSpeed = 100f;
	public float CoolOffTime = .5f;
	public float MuzzleLightFade = 20f;

	private float _time;
	private float _muzzleLight;
	private bool _right = true;

	private GameObject _menu;

	private LineRenderer _lineRenderer;

	void Start()
	{
		SetMuzzleLight();
		_menu = GameObject.Find("Menu");

		_lineRenderer = GetComponent<LineRenderer>();
	}

	private Vector3 _currentEnemyPoint;


	void Update ()
	{

		if (_menu != null && _menu.activeInHierarchy) return;
		
		if (Input.GetMouseButton(0) && _time >= CoolOffTime)
		{
			if (_right)
			{
				//GameObject bullet = Instantiate(Bullet, MuzzleRight.transform.position, Quaternion.identity);
				//bullet.GetComponent<Bullet>().SetTargetPosition(GetMousePositionWorldPoint(), LaserSpeed);

				_currentEnemyPoint = GetMousePositionWorldPoint();	
				_lineRenderer.enabled = true;
				_lineRenderer.SetPosition(0, MuzzleRight.transform.position);
				_lineRenderer.SetPosition(1, _currentEnemyPoint);
				_lineRenderer.endWidth = 0.5f;
				_lineRenderer.startWidth = 0.5f;
				
				MuzzleLightRight.intensity = _muzzleLight;
				MuzzleFlashRight.Play();
				BulletAudio.Play();
			}
			else
			{
				//GameObject bullet = Instantiate(Bullet, MuzzleLeft.transform.position, Quaternion.identity);
				//bullet.GetComponent<Bullet>().SetTargetPosition(GetMousePositionWorldPoint(), LaserSpeed);
				
				_currentEnemyPoint = GetMousePositionWorldPoint();
				_lineRenderer.enabled = true;
				_lineRenderer.SetPosition(0, MuzzleLeft.transform.position);
				_lineRenderer.SetPosition(1, _currentEnemyPoint);
				_lineRenderer.endWidth = 0.5f;
				_lineRenderer.startWidth = 0.5f;
				
				MuzzleLightLeft.intensity = _muzzleLight;
				MuzzleFlashLeft.Play();
				BulletAudio.Play();
			}

			_right = !_right;
			_time = 0f;
		}

		if (_lineRenderer != null)
		{
			_lineRenderer.SetPosition(0, Vector3.Lerp(_lineRenderer.GetPosition(0), _currentEnemyPoint, 5 * Time.deltaTime));
		}

		if (Vector3.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1)) < 10)
		{
			_lineRenderer.enabled = false;
		}
		else
		{
			_lineRenderer.endWidth = Mathf.Lerp(_lineRenderer.endWidth, 0, 5 * Time.deltaTime);
			_lineRenderer.startWidth = Mathf.Lerp(_lineRenderer.startWidth, 0, 5 * Time.deltaTime);
		}
		
		MuzzleLightLeft.intensity = Mathf.Lerp(MuzzleLightLeft.intensity, 0, MuzzleLightFade * Time.deltaTime);
		MuzzleLightRight.intensity = Mathf.Lerp(MuzzleLightRight.intensity, 0, MuzzleLightFade * Time.deltaTime);
		
		_time += Time.deltaTime;

	}

	public LayerMask IgnoreShip;

	Vector3 GetMousePositionWorldPoint()
	{

		RaycastHit hit;
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit, 500f, IgnoreShip)) {
			if (hit.collider.gameObject.GetComponent<Enemy>())
			{
				hit.collider.gameObject.GetComponent<Enemy>().EnemyShot();
				return hit.collider.gameObject.transform.position;
			}
		}

		//return Vector3.zero;

		var v3 = Input.mousePosition;
		v3.z = 50.0f;
		return Camera.main.ScreenToWorldPoint(v3);
	}

	void SetMuzzleLight()
	{
		_muzzleLight = MuzzleLightLeft.intensity;
		MuzzleLightLeft.intensity = 0;
		MuzzleLightLeft.enabled = true;
		
		_muzzleLight = MuzzleLightRight.intensity;
		MuzzleLightRight.intensity = 0;
		MuzzleLightRight.enabled = true;
	}

}
