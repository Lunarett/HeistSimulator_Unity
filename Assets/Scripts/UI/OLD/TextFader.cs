using UnityEngine;
using System.Collections;
using TMPro;

public class TextFader : MonoBehaviour
{
	[SerializeField] private float _duration = 1;
	[SerializeField] private bool _fadeOut;
	[SerializeField] private CountDown _counter;

	private TextMeshProUGUI _text;
	private Color _currentColor;
	private bool _doOnce = true;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		if(_text != null)
		{
			_currentColor = _text.color;
			StartCoroutine(FadeImage(_fadeOut));
		}
	}

	private void Update()
	{
		if(_doOnce)
		{
			_fadeOut = true;
			StartCoroutine(FadeImage(_fadeOut));
			_doOnce = false;
		}
	}

	IEnumerator FadeImage(bool fadeAway)
	{
		if (fadeAway)
		{
			for (float i = _duration; i >= 0; i -= Time.deltaTime)
			{
				_text.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, i);
				yield return null;
			}
		}
		else
		{
			for (float i = 0; i <= _duration; i += Time.deltaTime)
			{
				_text.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, i);
				
				yield return new WaitForSeconds(_duration);

				Destroy(gameObject);
			}
		}
	}
}
