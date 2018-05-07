using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCoinCue : MonoBehaviour
{

	private GameObject _coin;
	
	private void Start()
	{
		_coin = transform.parent.gameObject;
		transform.rotation = Quaternion.identity;
		_originalScale = transform.localScale;
		transform.localScale = Vector3.zero;
		LeanTween.moveY(gameObject, transform.position.y + 0.05f, 1f).setLoopPingPong().setEaseInOutSine();
	}

	private bool _isBig;
	private Vector3 _originalScale;
	private int _tweenID;

	void Update ()
	{	
		if (Vector3.Distance(Camera.main.transform.position, _coin.transform.position) < 2.5f)
		{
			if (!_isBig)
			{
				LeanTween.cancel(_tweenID);
				_tweenID = LeanTween.scale(gameObject, _originalScale, 0.1f).setEaseInOutSine().id;
				_isBig = true;
			}
			//gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			if (_isBig)
			{
				LeanTween.cancel(_tweenID);
				_tweenID = LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseInOutSine().id;
				_isBig = false;
			}
			//gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		
		transform.Rotate(Vector3.up, Time.deltaTime * 40f);
	}

	
}
