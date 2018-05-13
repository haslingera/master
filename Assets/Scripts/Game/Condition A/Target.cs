using UnityEngine;
using Random = UnityEngine.Random;
using EZCameraShake;

public class Target : MonoBehaviour
{
	public ParticleSystem Explosion;
	
	[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
	public Color StartColor = Color.green;
	[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
	public Color EndColor = Color.red;
	[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
	public Color FlashColor = Color.red;
	
	public float MinLifetime = 4f;
	public float MaxLifetime = 6f;
	public float BlinkTime = 0.5f;

	private float _lifeTime;
	private float _currentTime;
	private Material _asteroidMaterial;
	private float _initScale;
	
	private float _flashTime = 0.2f;
	private float _currentFlashTime = 1f;
	private bool _glowOn = true;
	private bool _glowChanged;

	private int _tweenId;
	private bool _blink;
	private bool _dead;

	private bool _canBeShot;

	void Start()
	{
		_lifeTime = Random.Range(MinLifetime, MaxLifetime)+ BlinkTime;
		_asteroidMaterial = GetComponent<MeshRenderer>().material;
		_asteroidMaterial.SetColor("_EmissionColor", StartColor);
		float newScale = transform.localScale.x * Random.Range(1f, 0.9f);
		transform.localScale = new Vector3(newScale,newScale,newScale);
		_initScale = transform.localScale.x;
		transform.localScale = new Vector3(_initScale - 1f, _initScale - 1f, _initScale - 1f);
	}

	private void Update()
	{

		if (_currentTime > _lifeTime)
		{
			DestroyTarget();
			_currentTime = 0;
		}

		if (_blink)
		{
			if (_currentFlashTime > _flashTime)
			{
				_glowOn = !_glowOn;
				_glowChanged = false;
				_currentFlashTime = 0;
			}

			if (_glowOn && !_glowChanged)
			{
				_glowChanged = true;
				_asteroidMaterial.SetColor("_EmissionColor", Color.Lerp(StartColor, EndColor,  1.0f - (_lifeTime - _currentTime) / _lifeTime));
				_flashTime = _flashTime * 0.6f;
			}

			if (!_glowOn && !_glowChanged)
			{
				_glowChanged = true;
				_asteroidMaterial.SetColor("_EmissionColor", FlashColor);
				_flashTime = _flashTime * 0.6f;
			}
			
			

			_currentFlashTime += Time.deltaTime;
		}
		else
		{
			_asteroidMaterial.SetColor("_EmissionColor", Color.Lerp(StartColor, EndColor,  1.0f - (_lifeTime - _currentTime) / _lifeTime));
		}

		if (GetComponent<MeshRenderer>().enabled)
		{
			_currentTime += Time.deltaTime;
		}
		
	}

	void UpdateScale(float scale)
	{
		transform.localScale = new Vector3(scale, scale, scale);
	}

	public void DestroyTarget()
	{
		if (!_dead)
		{
			_dead = true;
			LeanTween.cancel(_tweenId);
			LeanTween.value(gameObject, UpdateScale, transform.localScale.x, 0f, 0.2f).setEaseInBack().setOnComplete(Destroy);
		}
	}

	public void TargetShot(Bullet bullet)
	{
		if (!_dead && _canBeShot)
		{
			_dead = true;
			if (Explosion != null)
			{
				Explosion.Play();
			}
			CameraShaker.Instance.ShakeOnce(6f, 10f, 0.1f, 1f);
			LeanTween.cancel(_tweenId);
			GetComponent<MeshRenderer>().enabled = false;
						
			GameObject.Find("Enemy Explosion Sound").GetComponent<AudioSource>().Play();
			GameObject.Find("Score Sound").GetComponent<AudioSource>().Play();
			
			Destroy(GetComponent<PointOfInterest>());
			TimeScoreManager.Instance.IncreaseScore();
			
			DataRecorderNew.Instance.AddNewDataSet(bullet.BulletBirthTime, gameObject, DataRecorderNew.Action.ShotTriggered);
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.Shot);
			
			Invoke("Destroy", Explosion.main.duration + Explosion.main.startLifetimeMultiplier - 2.2f);
		}
	}
	
	public void TargetShot()
	{
		if (!_dead && _canBeShot)
		{
			_dead = true;
			if (Explosion != null)
			{
				Explosion.Play();
			}
			CameraShaker.Instance.ShakeOnce(6f, 10f, 0.1f, 1f);
			LeanTween.cancel(_tweenId);
			GetComponent<MeshRenderer>().enabled = false;
						
			GameObject.Find("Enemy Explosion Sound").GetComponent<AudioSource>().Play();
			GameObject.Find("Score Sound").GetComponent<AudioSource>().Play();
			
			Destroy(GetComponent<PointOfInterest>());
			TimeScoreManager.Instance.IncreaseScore();
			
			DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.Shot);
			
			Invoke("Destroy", Explosion.main.duration + Explosion.main.startLifetimeMultiplier - 2.2f);
		}
	}
	
	public void ShowTarget()
	{
		Invoke("FlashGlow", _lifeTime - BlinkTime);

		LeanTween.value(gameObject, UpdateScale, _initScale - 0.2f, _initScale, 0.2f).setEaseInOutCirc();
		_tweenId = LeanTween.value(gameObject, UpdateScale, _initScale, _initScale + 4, _lifeTime).setDelay(0.2f).setEaseOutCirc().id;
		
		GetComponent<MeshRenderer>().enabled = true;
		_canBeShot = true;
		
		DataRecorderNew.Instance.AddNewDataSet(Time.time, gameObject, DataRecorderNew.Action.Appeared);
	}

	void Destroy()
	{
		Destroy(gameObject);
	}
	
	void FlashGlow ()
	{
		_blink = true;
	}
}
