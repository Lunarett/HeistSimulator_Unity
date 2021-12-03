using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    public GameObject Camera;
    [Space]
    [Header("Money information")]
    [SerializeField] private float _playerWallet;
    [SerializeField] private float _safedMoney;
    [SerializeField] private float _onHitMoneyLossPercent;
    [Space]
    [Space]
    
    public float PlayerCurrentLoad;
    public float PlayerMaxLoadCapacity = 35000;

    public float WalletAmount { get => _playerWallet; set => _playerWallet = value; }
	public float SafedAmount { get => _safedMoney; set => _safedMoney = value; }

	[Space]
    [Space]
    [SerializeField] private HealthComponent _healthRef;

    [Space]
    [Tooltip("The maximum distance in which the player can interact with objects. ")]
    [SerializeField] private float _maxInteractDistance = 2;
    [Tooltip("The maximum distance in which objects get highlighted for the player.")]
    [SerializeField] private float _maxOutlineDistance = 5;
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _itemHeavyUI;

	private CollectableItem _collectableItem;
    private OutlineController _prevController;
    private OutlineController _currentController;
    private RaycastHit? _raycastHit;

    [SerializeField] private DoorBehaviour _doorBehaviourScript;

    private void Start()
    {
        _healthRef.HealthChanged += OnHealthChanged;

        _interactUI.SetActive(false);
        _itemHeavyUI.SetActive(false);
    }

    private void Update()
    {
        Raycast();
        OutlineInteractableObject();
        InteractWithObject();
    }

    private void OnHealthChanged(float arg1, bool getsHealed)
    {
        if (getsHealed == false)
        {
            _playerWallet *= 1- _onHitMoneyLossPercent;
            PlayerCurrentLoad *= 1 - _onHitMoneyLossPercent;
        }
    }

    private void Raycast()
    {
        RaycastHit[] raycastAll = Physics.RaycastAll(Camera.transform.position, Camera.transform.forward, _maxInteractDistance);
        raycastAll = raycastAll.OrderBy(x => x.distance).ToArray();

        Debug.DrawRay(Camera.transform.position, Camera.transform.forward, Color.blue, 3);

        if (raycastAll.Length > 0)
        {
            _raycastHit = raycastAll[0];
        }
        else
        {
            if (_raycastHit.HasValue)
            {
                HideOutline();
                _raycastHit = null;
            }
        }
    }

    private void OutlineInteractableObject()
    {
        if (_raycastHit.HasValue)
        {
            RaycastHit raycastHit = _raycastHit.Value;

            if (raycastHit.collider.CompareTag("InteractableObject") && raycastHit.collider != null)
            {
                _currentController = raycastHit.collider.GetComponent<OutlineController>();
                _collectableItem = raycastHit.transform.GetComponent<CollectableItem>();

                if (_prevController != _currentController)
                {
                    HideOutline();
                    ShowOutline();
                }

                _prevController = _currentController;
            }
            else
            {
                HideOutline();
            }
        }
    }

    private void InteractWithObject()
    {
        if (_raycastHit.HasValue)
        {

            if (Input.GetKeyDown(Keybindings.Interact))
            {
                RaycastHit raycastHit = _raycastHit.Value;

                if ((raycastHit.point - Camera.transform.position).magnitude < _maxInteractDistance)
                {
                    var interactable = raycastHit.transform.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        interactable.Interact(this);
                    }
                }
            }
        }
    }

    private void ShowOutline()
    {
        if (_currentController != null)
        {
            _currentController.ShowOutline();
        }

        if (_collectableItem != null)
        {
            if(PlayerCurrentLoad < PlayerMaxLoadCapacity)
                _interactUI.SetActive(true);
            else
            {
                _itemHeavyUI.SetActive(true);
            }
        }
	}

    private void HideOutline()
    {
        if (_prevController != null)
        {
            _prevController.HideOutline();
            _prevController = null;

            _itemHeavyUI.SetActive(false);
            _interactUI.SetActive(false);
        }
    }
}
