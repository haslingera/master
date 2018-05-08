using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitTextFormatter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	
	public StartAndFinishGame _StartAndFinishGame;

	public bool RespondToClickEvents = true;

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameObject.Find("Button Sound").GetComponent<AudioSource>().Play();
		LeanTween.colorText(gameObject.GetComponent<RectTransform>(), Color.white, .2f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//GameObject.Find("Button Sound").GetComponent<AudioSource>().Play();
		LeanTween.colorText(gameObject.GetComponent<RectTransform>(), new Color(1f, 1f, 1f, 0.1f), .2f);
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (RespondToClickEvents)
		{
			GameObject.Find("Confirmation Sound").GetComponent<AudioSource>().Play();
			
			if (GetComponent<Text>().text == "yes")
			{
				_StartAndFinishGame.QuitGame();
			}
			else
			{
				_StartAndFinishGame.UnPauseGame();
			}
		}
		
	}
}
