using UnityEngine;
using TMPro;

public class DrillUI : MonoBehaviour
{
	[SerializeField] private VaultDoorBehaviour _doorTimer;
	[SerializeField] private PlayerDeath _playerDeath;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (!_playerDeath.IsDead)
		{
			if (_doorTimer.CurrentTimer > 0)
			{
				_text.text = $"{Mathf.RoundToInt(_doorTimer.CurrentTimer)}s left";
			}
			else
			{
				_text.text = $"Bank Opened!";
			}
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
