using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
	[Header("Door Settings")]
	public bool _isOpen;
	[SerializeField] protected float _openAngle;
	[SerializeField] protected float _closeAngle;
	[Tooltip("How fast the door opens")]
	[SerializeField] protected float _rotationSpeed = 10;
	[SerializeField] protected float _targetAngle;
	[SerializeField] protected float _currentAngle;

	private void Awake()
	{
		if (_targetAngle == _openAngle)
		{
			_isOpen = true;
		}

		else if (_targetAngle == _closeAngle)
		{
			_isOpen = false;
		}
		_currentAngle = transform.localEulerAngles.y;
	}

	private void Update()
	{
		if (_targetAngle != _currentAngle)
		{
			float rotationAmount = Mathf.Min(_rotationSpeed * Time.deltaTime, Mathf.Abs(_targetAngle - _currentAngle));
			_currentAngle += Mathf.Sign(_targetAngle - _currentAngle) * rotationAmount;

			transform.localRotation = Quaternion.Euler(0, _currentAngle, 0);

			if (_isOpen) _closeAngle = _currentAngle;
		}
	}

	public virtual void SwitchDoorState()
	{
		_isOpen = !_isOpen;

		_targetAngle = _isOpen ? _openAngle : _closeAngle;
	}
}
