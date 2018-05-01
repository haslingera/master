
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitTextFormatter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	
	public GameObject CoinCanvas;
	public GameObject CursorCanvas;
	public GameObject ExitGameCanvas;
	public SmoothRotation _SmoothRotation1;
	public SmoothRotation _SmoothRotation2;

	public void OnPointerEnter(PointerEventData eventData)
	{
		LeanTween.colorText(gameObject.GetComponent<RectTransform>(), Color.white, .2f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		LeanTween.colorText(gameObject.GetComponent<RectTransform>(), new Color(1f, 1f, 1f, 0.1f), .2f);
	}
	
	public void OnPointerClick(PointerEventData eventData) // 3
	{
		if (GetComponent<Text>().text == "YES")
		{
			Application.Quit();
		}
		else
		{
			CoinCanvas.SetActive(true);
			CursorCanvas.SetActive(true);
			ExitGameCanvas.SetActive(false);
			
			Cursor.lockState = CursorLockMode.Locked;
			_SmoothRotation1.Rotate = true;
			_SmoothRotation2.Rotate = true;
		}
	}
}
