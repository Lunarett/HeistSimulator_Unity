using System.Collections;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
	[SerializeField] private GameManager _gameManager;
	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if(_gameManager.SpawnDelay > 0)
		{
			int newTime = Mathf.RoundToInt(_gameManager.SpawnDelay);
			_text.text = $"{newTime}s left";
		}
		else
		{
			_text.text = $"Police force has arrived!";
		}
	}
}
