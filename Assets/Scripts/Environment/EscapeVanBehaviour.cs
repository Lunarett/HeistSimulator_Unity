using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeVanBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerInteractController _playerScript;

    [SerializeField] private GameObject[] _vanRearDoors;
    [SerializeField] private VanRearDoorBehaviour _leftVanDoorScript;
    [SerializeField] private VanRearDoorBehaviour _rightVanDoorScript;

    [SerializeField] private GameObject[] _stolenGoods;
    [SerializeField] private float _moneyPerBag = 1;

    [FMODUnity.EventRef]
    [SerializeField] private string _deliver;

    private void Awake()
    {
        foreach (GameObject gameObject in _stolenGoods)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            _leftVanDoorScript.SwitchDoorState();
            _rightVanDoorScript.SwitchDoorState();

            if (_playerScript.WalletAmount > 0)
            {
                _playerScript.SafedAmount += _playerScript.WalletAmount;
                _playerScript.WalletAmount = 0;

                _playerScript.PlayerCurrentLoad = 0;

                UpdateVanContent();
                FMODUnity.RuntimeManager.PlayOneShotAttached(_deliver, gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _leftVanDoorScript.SwitchDoorState();
            _rightVanDoorScript.SwitchDoorState();
        }
    }

    private void UpdateVanContent()
    {
        int bagAmount = Mathf.Min(Mathf.RoundToInt(_playerScript.SafedAmount / _moneyPerBag), _stolenGoods.Length);

        for (int i = 0; i < _stolenGoods.Length; i++)
        {
            _stolenGoods[i].SetActive(i < bagAmount);
        }
    }
}
