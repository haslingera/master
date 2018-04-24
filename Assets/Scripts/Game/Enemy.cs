﻿using UnityEngine;
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
		_lifeTime = Random.Range(MinLifetime, MaxLifetime);
		_asteroidMaterial = GetComponent<MeshRenderer>().material;
		_asteroidMaterial.SetColor("_EmissionColor", StartColor);
		_initScale = transform.localScale.x;
		transform.localScale = Vector3.zero;

		LeanTween.value(gameObject, UpdateScale, 0f, _initScale, 0.5f).setEaseInOutCirc();
		_tweenId = LeanTween.value(gameObject, UpdateScale, _initScale, _initScale + 4, _lifeTime).setDelay(0.5f).setEaseOutCirc().id;
		
		Invoke("FlashGlow", _lifeTime - 2.0f);
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
			}

			if (!_glowOn && !_glowChanged)
			{
				_glowChanged = true;
				_asteroidMaterial.SetColor("_EmissionColor", FlashColor);
				_flashTime = _flashTime * 0.9f;
			}

			_currentFlashTime += Time.deltaTime;
		}
		else
		{
			_asteroidMaterial.SetColor("_EmissionColor", Color.Lerp(StartColor, EndColor,  1.0f - (_lifeTime - _currentTime) / _lifeTime));
		}
		
		_currentTime += Time.deltaTime;
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
			LeanTween.value(gameObject, UpdateScale, transform.localScale.x, 0f, 0.2f).setEaseInBack().setDestroyOnComplete(gameObject);
		}
	}

	public void EnemyShot()
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

			Invoke("Destroy", Explosion.duration + Explosion.startLifetime);
			
			GetComponent<AudioSource>().Play();
			
			TimeScoreManager.Instance.IncreaseScore();

		}
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