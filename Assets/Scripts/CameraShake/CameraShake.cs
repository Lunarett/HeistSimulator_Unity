using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	[SerializeField] private bool _debugMode = false;

	[SerializeField] private float _shakeAmount;
	[SerializeField] private float _shakeDuration;

	
	[SerializeField] private float _shakePercentage;
	[SerializeField] private float _startAmount;
	[SerializeField] private float _startDuration;

	[SerializeField] private bool _isRunning = false;

	[SerializeField] private bool smooth;
	[SerializeField] private float smoothAmount = 5f;

	void Start()
	{
		if (_debugMode) ShakeCamera();
	}

	void ShakeCamera()
	{
		_startAmount = _shakeAmount;
		_startDuration = _shakeDuration;

		if (!_isRunning) StartCoroutine(Shake());
	}

	public void ShakeCamera(float amount, float duration)
	{
		_shakeAmount += amount;
		_startAmount = _shakeAmount;
		_shakeDuration += duration;
		_startDuration = _shakeDuration;

		if (!_isRunning) StartCoroutine(Shake());
	}

	IEnumerator Shake()
	{
		_isRunning = true;

		while (_shakeDuration > 0.01f)
		{
			Vector3 rotationAmount = Random.insideUnitSphere * _shakeAmount;
			rotationAmount.z = 0;

			_shakePercentage = _shakeDuration / _startDuration;
			_shakeAmount = _startAmount * _shakePercentage;
			_shakeDuration = Mathf.Lerp(_shakeDuration, 0, Time.deltaTime);


			if (smooth)
				transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);
			else
				transform.localRotation = Quaternion.Euler(rotationAmount);

			yield return null;
		}
		transform.localRotation = Quaternion.identity;
		_isRunning = false;
	}
}