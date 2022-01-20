using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private HealthComponent _health;
	[SerializeField] private Slider slider;
	[SerializeField] private float _speed;

	private Coroutine _updateHealthCoroutine;
	private float _targetHealth;

	private void Start()
	{
		SetMaxHealth(_health.MaxHealth);
		_health.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(float amount, bool isHealing)
	{
		slider.value = _targetHealth;

		if (_updateHealthCoroutine != null)
			StopCoroutine(_updateHealthCoroutine);

		_targetHealth = amount;

		if(gameObject.activeInHierarchy)
			_updateHealthCoroutine = StartCoroutine(UpdateHealth());
	}

	void SetMaxHealth(float maxHealth)
	{
		slider.maxValue = maxHealth;
		slider.value = maxHealth;
		_targetHealth = maxHealth;
	}

	IEnumerator UpdateHealth()
	{
		float startingValue = slider.value;
		float timer = 0;

		while (true)
		{
			timer += Time.deltaTime * _speed;

			slider.value = Mathf.Lerp(startingValue, _targetHealth, timer);

			if (timer >= 1f)
				break;

			yield return null;
		}
		slider.value = _targetHealth;
	}
}