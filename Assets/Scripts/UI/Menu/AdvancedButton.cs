using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AdvancedButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
	[SerializeField] private Image _buttonImage;
	[SerializeField] private TMP_Text _buttonText;
	[Space]
	[Header("Color Properties")]
	[SerializeField] private Color _normalColor = Color.white;
	[SerializeField] private Color _hoverColor = Color.white;
	[SerializeField] private Color _pressedColor = Color.white;
	[SerializeField] private Color _releasedColor = Color.white;
	[Space]
	[Header("Scale Properties")]
	[SerializeField] private bool _scaleOnHover = false;
	[SerializeField] private float _scaleAmount = 1.5f;
	[SerializeField] private float _scaleDuration = 0.2f;


	public event Action<PointerEventData, Image, TMP_Text> OnClicked;
	public event Action<PointerEventData, Image, TMP_Text> OnEnter;
	public event Action<PointerEventData, Image, TMP_Text> OnExit;
	public event Action<PointerEventData, Image, TMP_Text> OnPressed;
	public event Action<PointerEventData, Image, TMP_Text> OnReleased;

	private void Start()
	{
		_buttonImage.color = _normalColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClicked?.Invoke(eventData, _buttonImage, _buttonText);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnEnter?.Invoke(eventData, _buttonImage, _buttonText);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnExit?.Invoke(eventData, _buttonImage, _buttonText);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnPressed?.Invoke(eventData, _buttonImage, _buttonText);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnReleased?.Invoke(eventData, _buttonImage, _buttonText);
	}
}