using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	[SerializeField] private GameObject _playerPrefab;

	[Header("Wallet")]
	[SerializeField] private TextMeshProUGUI _walletAmount;
	[SerializeField] private TextMeshProUGUI _safedAmount;

	[Header("Weapon")]
	[SerializeField] private TextMeshProUGUI _weaponName;
	[SerializeField] private TextMeshProUGUI _ammo;
	[SerializeField] private Image _weaponIcon;

	private PlayerCharacter _playerCharacterRef;
	private PlayerInteractController _interactController;

	private void Awake()
	{
		_playerCharacterRef = _playerPrefab.GetComponent<PlayerCharacter>();
		_interactController = _playerPrefab.GetComponent<PlayerInteractController>();
	}

	private void Update()
	{
		UpdateWeaponInfo();
		UpdateWalletInfo();
	}

	private void UpdateWeaponInfo()
	{
		_ammo.text = $"{_playerCharacterRef.CurrentWeapon.CurrentAmmo} / {_playerCharacterRef.CurrentWeapon.MagazineSize}";

		_weaponName.text = _playerCharacterRef.CurrentWeapon.Name;

		_weaponIcon.sprite = _playerCharacterRef.CurrentWeapon.WeaponIcon;
	}

	private void UpdateWalletInfo()
	{
		_walletAmount.text = _interactController.WalletAmount.ToString("C");
		_safedAmount.text = _interactController.SafedAmount.ToString("C");
	}
}
