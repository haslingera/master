using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCue : MonoBehaviour
{

	private GameObject _coin;
	private bool _isBig;
	private Vector3 _originalScale;
	private float _originalIntensityLight;
	private int _tweenID;
	private int _tweenIDLight;
	private float _animationTime = 0.2f;
	
	
	private void Start()
	{
		_coin = transform.parent.gameObject;
		transform.rotation = Quaternion.identity;
		
		_originalScale = transform.localScale;
		_originalIntensityLight = GetLightIntensity();
		
		transform.localScale = Vector3.zero;
		SetLightIntensity(0);
		
		LeanTween.moveY(gameObject, transform.position.y + 0.05f, 1f).setLoopPingPong().setEaseInOutSine();
		LeanTween.moveY(gameObject, transform.GetChild(0).transform.position.y + 0.05f, 1f).setLoopPingPong().setEaseInOutSine();
	}

	
	void Update ()
	{	
		if (Vector3.Distance(Camera.main.transform.position, _coin.transform.position) < 2.5f)
		{
			if (!_isBig)
			{
				LeanTween.cancel(_tweenID);
				LeanTween.cancel(_tweenIDLight);
				
				_tweenID = LeanTween.scale(gameObject, _originalScale, 0.1f).setEaseInOutSine().id;
				_tweenIDLight = LeanTween.value(gameObject, SetLightIntensity, GetLightIntensity(), _originalIntensityLight, _animationTime).setEaseInOutSine().id;
				
				_isBig = true;
			}
		}
		else
		{
			if (_isBig)
			{
				LeanTween.cancel(_tweenID);
				LeanTween.cancel(_tweenIDLight);
				
				_tweenID = LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseInOutSine().id;
				_tweenIDLight = LeanTween.value(gameObject, SetLightIntensity, GetLightIntensity(), 0, _animationTime).setEaseInOutSine().id;
				
				_isBig = false;
			}
		}
		
		transform.Rotate(Vector3.up, Time.deltaTime * 40f);
	}

	public void SetLightIntensity(float value)
	{
		transform.GetChild(0).GetComponent<Light>().intensity = value;
	}
	
	private float GetLightIntensity()
	{
		return transform.GetChild(0).GetComponent<Light>().intensity;
	}

	
}
