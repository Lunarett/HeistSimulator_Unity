using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[FMODUnity.EventRef]
	[SerializeField] private string _music;
	[SerializeField] private GameManager _alarm;
	[SerializeField] private VaultDoorBehaviour _vault;
	[SerializeField] private HealthComponent _healthComp;

	private FMOD.Studio.EventInstance _eventInst;

	private void Awake()
	{
		_healthComp.Death += OnDeath;
		_alarm.UpdateState += OnStateChanged;
		_vault.VaultDoorState += VaultDoorOpened;
	}

	public void OnDeath()
	{
		_eventInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	private void VaultDoorOpened(int stateID)
	{
		_eventInst.setParameterByName("State", stateID);
	}

	private void OnStateChanged(int stateID)
	{
		_eventInst.setParameterByName("State", stateID);
	}

	private void Start()
	{
		_eventInst = FMODUnity.RuntimeManager.CreateInstance(_music);
		_eventInst.start();
	}
}

