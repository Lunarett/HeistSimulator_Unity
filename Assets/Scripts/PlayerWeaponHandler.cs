using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
	[SerializeField] private List<Weapon> _weaponList;

	private Weapon _primaryWeapon;
	private Weapon _secondaryWeapon;
	private Weapon _currentWeapon;

	public Weapon GetCurrentWeapon() => _currentWeapon;

	private void Start()
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

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Secondary();
		}
	}
	
	private void Primary()
	{
		_currentWeapon = _primaryWeapon;
		_secondaryWeapon.gameObject.SetActive(false);
		_primaryWeapon.gameObject.SetActive(true);
	}

	private void Secondary()
	{
		_currentWeapon = _secondaryWeapon;
		_primaryWeapon.gameObject.SetActive(false);
		_secondaryWeapon.gameObject.SetActive(true);
	}

	public void LoadPrimary(EWeaponType weaponType)
	{
		for (int i = 0; i < _weaponList.Count; i++)
		{
			if(_weaponList[i].GetWeaponType() == weaponType)
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
			if (_weaponList[i].GetWeaponType() == weaponType)
			{
				_secondaryWeapon = _weaponList[i];
				break;
			}
		}
	}
}
