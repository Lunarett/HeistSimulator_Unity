using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
	[SerializeField] private List<Weapon> _weaponList;

	private Weapon _primaryWeapon;
	private Weapon _secondaryWeapon;
	private Weapon _currentWeapon;

	public Weapon PrimaryWeapon { get => _primaryWeapon; }
	public Weapon SecondaryWeapon { get => _secondaryWeapon; }
	public Weapon CurrentWeapon { get => _currentWeapon; }

	public event System.Action<Weapon> OnSwitchSlot;

	private void Awake()
	{
		LoadPrimary(EWeaponType.AK47);
		LoadSecondary(EWeaponType.Glock17);

		Primary();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Primary();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Secondary();
		}
	}

	private void Primary()
	{
		_currentWeapon = _primaryWeapon;
		_secondaryWeapon.gameObject.SetActive(false);
		_primaryWeapon.gameObject.SetActive(true);

		OnSwitchSlot?.Invoke(_currentWeapon);
	}

	private void Secondary()
	{
		_currentWeapon = _secondaryWeapon;
		_primaryWeapon.gameObject.SetActive(false);
		_secondaryWeapon.gameObject.SetActive(true);

		OnSwitchSlot?.Invoke(_currentWeapon);
	}

	public void LoadPrimary(EWeaponType weaponType)
	{
		for (int i = 0; i < _weaponList.Count; i++)
		{
			if (_weaponList[i].WeaponType == weaponType)
			{
				_primaryWeapon = _weaponList[i];
				break;
			}
		}
	}

	public void LoadSecondary(EWeaponType weaponType)
	{
		for (int i = 0; i < _weaponList.Count; i++)
		{
			if (_weaponList[i].WeaponType == weaponType)
			{
				_secondaryWeapon = _weaponList[i];
				break;
			}
		}
	}
}
