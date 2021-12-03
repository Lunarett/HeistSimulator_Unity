using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Camera _gunCamera;
	[SerializeField] private Camera _deathCam;
	[SerializeField] private Transform _deathCamPos;
	[SerializeField] private GameObject _arms;
	[SerializeField] private GameObject _character;
	[SerializeField] private GameObject _canvas;
	[SerializeField] private GameObject _wastedPrefab;
	[FMODUnity.EventRef]
	[SerializeField] private string _deathSound;

	public bool IsDead { get; private set; }

	private PlayerCharacter _playerChar;
	private bool _doOnce = true;

	private void Awake()
	{
		_playerChar = GetComponent<PlayerCharacter>();

		HealthComponent _health = GetComponent<HealthComponent>();

		if (_health == null)
			Destroy(this);
		else
			_health.Death += OnDeath;
			
	}

	private void OnDeath()
	{
		if(_doOnce)
		{
			FMODUnity.RuntimeManager.PlayOneShot(_deathSound, gameObject.transform.position);
			IsDead = true;

			_playerChar.enabled = false;

			_mainCamera.enabled = false;
			_gunCamera.enabled = false;

			_arms.SetActive(false);

			_deathCam.transform.position = _deathCamPos.position;
			_deathCam.transform.rotation = _deathCamPos.rotation;

			_canvas.SetActive(false);

			_deathCam.targetDisplay = 0;

			Time.timeScale = 0.5f;

			Instantiate(_wastedPrefab, gameObject.transform);
			Instantiate(_character, gameObject.transform.position, gameObject.transform.rotation);
			_doOnce = false;
		}
	}
}
