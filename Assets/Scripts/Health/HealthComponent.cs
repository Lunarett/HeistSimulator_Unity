using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
	[Header("Health Settings")]
	[SerializeField] private float _maxHealth = 100;

	[Space]
	[Header("Regenerations Properties")]
	[SerializeField] private float _regenAmount;

	public event System.Action Death;
	public event System.Action<float, bool> HealthChanged;

	private float _currentHealth;
	public float MaxHealth { get => _maxHealth; }

	private void Awake()
	{
		_currentHealth = _maxHealth;
	}

	void Start()
	{
		if (_regenAmount > 0)
			StartCoroutine(Regenerate());
	}

	IEnumerator Regenerate()
	{
		while (true && _currentHealth > 5)
		{
			Heal(_regenAmount * Time.deltaTime);

			yield return null;
		}
	}

	public void Heal(float heal)
	{
		_currentHealth = Mathf.Clamp(_currentHealth += heal, 0, _maxHealth);
		HealthChanged?.Invoke(_currentHealth, true);
	}

	public void Damage(float damage)
	{
		_currentHealth = Mathf.Clamp(_currentHealth -= damage, 0, _maxHealth);
		HealthChanged?.Invoke(_currentHealth, false);

		if (_currentHealth <= 0)
			Death?.Invoke();
	}
}