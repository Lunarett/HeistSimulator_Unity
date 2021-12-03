using Assets.Scripts.AIPathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerCharacter _playerChar;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private float _secondWaveDelay = 30;
	[Space]
	[Header("Spawn Prefab List")]
	[SerializeField] private List<GameObject> _vehicleList = new List<GameObject>();
	[SerializeField] private List<GameObject> _enemyList = new List<GameObject>();
	[Space]
	[Header("Spawn Locations List")]
	[SerializeField] private List<Transform> _vehicleLocationList = new List<Transform>();
	[SerializeField] private List<Transform> _enemyLocationList = new List<Transform>();
	[Space]
	[Header("Spawn UI")]
	[SerializeField] private GameObject _detectedUIPrefab;
	[Space]
	[Header("Debug Settings")]
	[SerializeField] bool _noDetection;
	[Space]
	[Header("FMOD Sounds")]
	[FMODUnity.EventRef]
	[SerializeField] private string _bell;
	[FMODUnity.EventRef]
	[SerializeField] private string _progressUI;
	[SerializeField] private Transform _bellLocation;

	public event Action<int> UpdateState;

	private bool _doOnce = true;
	private bool _inBank;
	private bool _isDetected;

	public float SpawnDelay { get => _spawnDelay; }

	private void Update()
	{
		if (_inBank && !_playerChar.IsHolstered) HasBeenDetected();
		if(_isDetected) _spawnDelay -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_inBank = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && _doOnce)
		{
			_inBank = false;
		}
	}

	public void HasBeenDetected()
	{
		if (_doOnce && !_noDetection)
		{
			UpdateState?.Invoke(1);
			FMODUnity.RuntimeManager.PlayOneShotAttached(_bell, _bellLocation.gameObject);
			FMODUnity.RuntimeManager.PlayOneShotAttached(_progressUI, gameObject);

			_detectedUIPrefab.SetActive(true);

			StartCoroutine(AlarmTriggered());
			_doOnce = false;
			_isDetected = true;
		}
	}

	IEnumerator AlarmTriggered()
	{
		AIHandler.Instance.AggroAll();

		yield return new WaitForSeconds(_spawnDelay);

		for (int i = 0; i < _vehicleLocationList.Count; i++)
		{
			int chooseRandomVehicle = UnityEngine.Random.Range(0, _vehicleList.Count);

			Instantiate(_vehicleList[chooseRandomVehicle], _vehicleLocationList[i].position, _vehicleLocationList[i].rotation);
		}

		for (int i = 0; i < _enemyLocationList.Count; i++)
		{
			int chooseRandomEnemy = UnityEngine.Random.Range(0, _enemyList.Count);

			Instantiate(_enemyList[chooseRandomEnemy], _enemyLocationList[i].position, _enemyLocationList[i].rotation);

		}
		yield return new WaitForSeconds(0.1f);

		AIHandler.Instance.AggroAll();
		
		yield return new WaitForSeconds(2);

		_detectedUIPrefab.SetActive(false);

		float spawnMultiplier = 1;

		while (true)
        {
			yield return new WaitForSeconds(_secondWaveDelay);

			Debug.Log("Next wave of cops is spawning");
			Debug.Log(_enemyLocationList.Count * spawnMultiplier);

			for (int i = 0; i < _enemyLocationList.Count * spawnMultiplier; i++)
			{
				int chooseRandomEnemy = UnityEngine.Random.Range(0, _enemyList.Count);

				Instantiate(_enemyList[chooseRandomEnemy], _enemyLocationList[i % _enemyLocationList.Count].position, _enemyLocationList[i % _enemyLocationList.Count].rotation);
			}

			spawnMultiplier++;
			_secondWaveDelay += 10;
			yield return new WaitForSeconds(0.1f);
			AIHandler.Instance.AggroAll();
        }
	}
}