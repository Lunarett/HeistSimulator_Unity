using System;
using UnityEngine;

public class VaultDoorBehaviour : DoorBehaviour
{
	[SerializeField] private GameObject _vaultDrill;
	[SerializeField] private GameObject _vaultDoorHandle;
	[SerializeField] private GameObject _drillUIPrefab;
	[SerializeField] private float _timeLeft;

	[FMODUnity.EventRef]
	[SerializeField] private string _openDoor;
	[FMODUnity.EventRef]
	[SerializeField] private string _drillSound;

	private FMOD.Studio.EventInstance _eventInst;
	private DrillUI _drillUI;
	private bool _doOnce = true;
	private bool _doOnce2 = true;

	public float CurrentTimer { get => _timeLeft; }
	public event Action<int> VaultDoorState;

	private void Awake()
	{
		_eventInst = FMODUnity.RuntimeManager.CreateInstance(_drillSound);

		_drillUIPrefab.SetActive(false);
		_vaultDrill.SetActive(false);

		if (_targetAngle == _openAngle)
		{
			_isOpen = true;
		}

		else
		{
			_isOpen = false;
		}
		_currentAngle = transform.localEulerAngles.y;
	}

	// Update is called once per frame
	private void Update()
	{
		if (_targetAngle != _currentAngle)
		{
			if (_timeLeft <= 0)
			{
				if(_doOnce2)
				{
					FMODUnity.RuntimeManager.PlayOneShotAttached(_openDoor, gameObject);
					_eventInst.setParameterByName("DrillState", 1);
					VaultDoorState?.Invoke(2);
					_doOnce2 = false;
				}

				_drillUIPrefab.SetActive(false);

				_vaultDoorHandle.transform.Rotate(Vector3.forward, Time.deltaTime * 140f);

				float rotationAmount = Mathf.Min(_rotationSpeed * Time.deltaTime, Mathf.Abs(_targetAngle - _currentAngle));
				_currentAngle += Mathf.Sign(_targetAngle - _currentAngle) * rotationAmount;

				transform.localRotation = Quaternion.Euler(0, _currentAngle, 0);
			}
			else
			{
				if (_doOnce)
				{
					_vaultDrill.SetActive(true);
					FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventInst, _vaultDrill.transform, (Rigidbody)null);
					_eventInst.start();

					_drillUIPrefab.SetActive(true);
					_doOnce = false;
				}
			}

			_timeLeft -= Time.deltaTime;
		}
		if (_isOpen) _closeAngle = _currentAngle;
	}

    public override void SwitchDoorState()
    {
		_isOpen = true;
		_targetAngle = _openAngle;
	}
}
