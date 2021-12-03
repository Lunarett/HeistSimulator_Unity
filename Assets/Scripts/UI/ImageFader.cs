using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
	[SerializeField] private float _duration = 1;
	[SerializeField] private float _maxValue = 1;
	[SerializeField] private bool _fadeOut;
	[SerializeField] private CountDown _counter;

	private Image _image;
	private Color _currentColor;
	private bool _doOnce = true;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	private void Start()
	{
		_currentColor = _image.color;
		StartCoroutine(FadeImage(_fadeOut));
	}

	IEnumerator FadeImage(bool fadeAway)
	{
		if (fadeAway)
		{
			for (float i = _duration; i >= 0; i -= Time.deltaTime)
			{
				_image.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, i);
				yield return null;
			}
		}
		else
		{
			for (float i = 0; i <= _duration; i += Time.deltaTime)
			{
				_image.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, i);

				yield return new WaitForSeconds(_duration);

				Destroy(gameObject);
			}
		}
	}
}
