using UnityEngine;

public class SetAlarm : MonoBehaviour
{
	[SerializeField] private GameManager _gameManager;

	private bool _detected;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player")) _detected = true;
	}

	private void Update()
	{
		if(_detected) _gameManager.HasBeenDetected();
	}
}
