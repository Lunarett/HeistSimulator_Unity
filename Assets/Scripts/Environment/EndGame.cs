using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Interactable
{
    [SerializeField] private GameObject _gameCompleteUIRef;
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private GameObject _completeWarningUI;

    public override void Interact(PlayerInteractController interactor)
    {
        _gameCompleteUIRef.SetActive(true);
        _gameCompleteUIRef.GetComponent<CompleteGameUI>().UpdateUI((int)interactor.SafedAmount);

        Time.timeScale = 0.1f;

        _playerCharacter.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Destroy(gameObject);
    }

	private void OnTriggerEnter(Collider other)
	{
        _completeWarningUI.SetActive(true);
	}

	private void OnTriggerExit(Collider other)
	{
        _completeWarningUI.SetActive(false);
    }
}
