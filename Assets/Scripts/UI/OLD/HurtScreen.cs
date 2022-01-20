using UnityEngine;
using UnityEngine.UI;

public class HurtScreen : MonoBehaviour
{
	[SerializeField] private HealthComponent _healthComp;
	[SerializeField] private CameraShake _camShake;
	[SerializeField] private Image _image;
	[SerializeField] private float _smoothness = 3;

	private Color _damageColor;
	private float _timer;

	private void Awake()
	{
		_healthComp.HealthChanged += DealDamage;
		_damageColor = _image.color;
	}

	private void DealDamage(float amount, bool isHealing)
	{
		if (!isHealing)
		{
			_timer = 0.1f;
			_camShake.ShakeCamera(0.1f, 0.0015f);
		}
	}

	private void Update()
	{
		if(_timer > 0)
		{
			gameObject.SetActive(true);
			_image.color = _damageColor;
		}
		else
		{
			_image.color = Color.Lerp(_image.color, Color.clear, _smoothness * Time.deltaTime);
		}

		_timer -= Time.deltaTime;
	}
}
