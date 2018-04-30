using UnityEngine;
using Random = UnityEngine.Random;
using EZCameraShake;

public class Enemy : MonoBehaviour
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

	void Start()
	{
		_lifeTime = Random.Range(MinLifetime, MaxLifetime)+ BlinkTime;
		_asteroidMaterial = GetComponent<MeshRenderer>().material;
		_asteroidMaterial.SetColor("_EmissionColor", StartColor);
		_initScale = transform.localScale.x;
		transform.localScale = Vector3.zero;

		DataRecorder.Instance.CreateNewDataSet();
	}

	private void Update()
	{

		if (_currentTime > _lifeTime)
		{
			DestroyEnemy();
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

	public void DestroyEnemy()
	{
		if (!_dead)
		{
			_dead = true;
			LeanTween.cancel(_tweenId);
			LeanTween.value(gameObject, UpdateScale, transform.localScale.x, 0f, 0.2f).setEaseInBack().setOnComplete(Destroy);
		}
	}

	public void EnemyShot(Bullet bullet)
	{
		if (!_dead)
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
			
			DataRecorder.Instance.GetCurrentDataSet().ShotTriggered = true; 
			DataRecorder.Instance.GetCurrentDataSet().TimeShotTriggered = bullet.BulletBirthTime;
			
			DataRecorder.Instance.GetCurrentDataSet().Shot = true; 
			DataRecorder.Instance.GetCurrentDataSet().TimeShot = Time.time;
			
			Invoke("Destroy", Explosion.main.duration + Explosion.main.startLifetimeMultiplier - 2.2f);
		}
	}
	
	public void ShowEnemy()
	{
		Invoke("FlashGlow", _lifeTime - BlinkTime);

		LeanTween.value(gameObject, UpdateScale, 0f, _initScale, 0.5f).setEaseInOutCirc();
		_tweenId = LeanTween.value(gameObject, UpdateScale, _initScale, _initScale + 4, _lifeTime).setDelay(0.5f).setEaseOutCirc().id;
		
		GetComponent<MeshRenderer>().enabled = true;
		DataRecorder.Instance.GetCurrentDataSet().TimeAppeared = Time.time;
	}

	void Destroy()
	{
		DataRecorder.Instance.FinalizeCurrentDataSet();
		Destroy(gameObject);
	}
	
	void FlashGlow ()
	{
		_blink = true;
	}
}
