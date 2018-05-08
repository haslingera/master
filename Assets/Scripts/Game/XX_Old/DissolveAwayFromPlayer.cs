using UnityEngine;

public class DissolveAwayFromPlayer : MonoBehaviour
{
	public GameObject Player;
	private Renderer _renderer;
	
	private bool _tweenedBig;
	private bool _tweenedSmall;
	
	private Vector3 _currentClosestVector = Vector3.zero;
	private PlayerGuidanceSystem _pgs;

	private float _smallSize = 4f;
	private float _bigSize = 14f;
	private float _animationDuration = 0.5f;

	void Start ()
	{
		_renderer = GetComponent<Renderer>();
		_pgs = GameObject.Find("PlayerGuidanceSystem").GetComponent<PlayerGuidanceSystem>();
	}

	private void Update()
	{
		if (_tweenedSmall)
		{
			_renderer.sharedMaterial.SetVector("_Center", new Vector4(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z, 0));
		}

		if (_pgs.IsWithinRadius())
		{
			TweenBig();
		}
		else
		{
			TweenSmall();
		}
	}

	public void TweenBig()
	{
		if (!_tweenedBig)
		{
			_currentClosestVector = _pgs.VectorOfClosestMission();
			_tweenedSmall = false;
			_tweenedBig = true;
			LeanTween.value(gameObject, TweenValue, _smallSize, _bigSize, _animationDuration).setOnComplete(TweenBigComplete).setEaseOutBack();
			LeanTween.value(gameObject, TweenVectorBig, 0f, 1f, _animationDuration).setEaseOutQuint();
		}
	}

	public void TweenSmall()
	{
		if (!_tweenedSmall)
		{
			_tweenedSmall = true;
			_tweenedBig = false;
			LeanTween.value(gameObject, TweenValue, _bigSize, _smallSize, _animationDuration).setOnComplete(TweenSmallComplete).setEaseOutQuint();
			LeanTween.value(gameObject, TweenVectorSmall, 0f, 1f, _animationDuration).setEaseOutQuint();
		}
	}
	
	private void TweenValue(float value)
	{		
		_renderer.sharedMaterial.SetFloat("_Distance", value);
	}
	
	private void TweenVectorSmall(float value)
	{		
		Vector3 lerp = Vector3.Lerp(_currentClosestVector, Player.transform.position, value);
		_renderer.sharedMaterial.SetVector("_Center", new Vector4(lerp.x, lerp.y, lerp.z, 0));
	}
	
	private void TweenVectorBig(float value)
	{		
		Vector3 lerp = Vector3.Lerp(Player.transform.position, _currentClosestVector, value);
		_renderer.sharedMaterial.SetVector("_Center", new Vector4(lerp.x, lerp.y, lerp.z, 0));
	}

	private void TweenSmallComplete()
	{
		_tweenedSmall = true;
	}
	
	private void TweenBigComplete()
	{
		_tweenedBig = true;
	}
	
	private void OnApplicationQuit()
	{
		_renderer.sharedMaterial.SetFloat("_Distance", _bigSize);
		_renderer.sharedMaterial.SetVector("_Center", new Vector4(0, 0, 4, 0));
	}
}
