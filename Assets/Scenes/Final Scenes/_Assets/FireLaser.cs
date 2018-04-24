using UnityEngine;

public class FireLaser : MonoBehaviour
{

	public GameObject Bullet;
	
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

	void Start()
	{
		SetMuzzleLight();
	}


	void Update () {
		
		if (Input.GetMouseButton(0) && _time >= CoolOffTime)
		{
			if (_right)
			{
				GameObject bullet = Instantiate(Bullet, MuzzleRight.transform.position, Quaternion.identity);
				bullet.GetComponent<Bullet>().SetTargetPosition(GetMousePositionWorldPoint(), LaserSpeed);
				MuzzleLightRight.intensity = _muzzleLight;
				MuzzleFlashRight.Play();
			}
			else
			{
				GameObject bullet = Instantiate(Bullet, MuzzleLeft.transform.position, Quaternion.identity);
				bullet.GetComponent<Bullet>().SetTargetPosition(GetMousePositionWorldPoint(), LaserSpeed);
				MuzzleLightLeft.intensity = _muzzleLight;
				MuzzleFlashLeft.Play();
			}

			/*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log(hit.transform.name);
			}*/

			_right = !_right;
			_time = 0f;
		}
		
		MuzzleLightLeft.intensity = Mathf.Lerp(MuzzleLightLeft.intensity, 0, MuzzleLightFade * Time.deltaTime);
		MuzzleLightRight.intensity = Mathf.Lerp(MuzzleLightRight.intensity, 0, MuzzleLightFade * Time.deltaTime);
		
		_time += Time.deltaTime;

	}

	Vector3 GetMousePositionWorldPoint()
	{
		var v3 = Input.mousePosition;
		v3.z = 300.0f;
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
