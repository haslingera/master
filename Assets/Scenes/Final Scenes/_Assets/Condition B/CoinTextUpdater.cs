using UnityEngine;
using UnityEngine.UI;

public class CoinTextUpdater : MonoBehaviour
{

	private Text _text;

	private int _oldScore;

	private Vector2 _originalPosition;
	
	// Use this for initialization
	void Start ()
	{
		_text = GetComponent<Text>();
		_text.enabled = false;
		_originalPosition = GetComponent<RectTransform>().anchoredPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (TimeScoreManager.Instance.Score > 0)
		{
			_text.enabled = true;
		}

		if (_oldScore != TimeScoreManager.Instance.Score)
		{
			_oldScore = TimeScoreManager.Instance.Score;

			if (TimeScoreManager.Instance.Score == 1)
			{
				_text.text = TimeScoreManager.Instance.Score + " Coin";
			}
			else
			{
				_text.text = TimeScoreManager.Instance.Score + " Coins";
			}
			
			LeanTween.moveY(GetComponent<RectTransform>(), _originalPosition.y + 10f, .1f).setEaseOutBack();
			LeanTween.moveY(GetComponent<RectTransform>(), _originalPosition.y, .1f).setEaseOutBack().setDelay(.1f);
			
		}
	}
}
