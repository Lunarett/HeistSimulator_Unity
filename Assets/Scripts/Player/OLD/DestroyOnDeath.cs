using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
	private void Awake()
	{
		HealthComponent _health = GetComponent<HealthComponent>();

		if (_health == null)
			Destroy(this);
		else
			_health.Death += OnDeath;
	}

	private void OnDeath()
	{
		Destroy(gameObject);
	}
}
