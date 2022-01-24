using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
	private PlayerController _playerController;
	private PlayerWeaponHandler _weaponHandler;
	private Weapon _weapon;
	private Animator _anim;

	private EMovementStates _movementState;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		_weaponHandler = GetComponent<PlayerWeaponHandler>();
		_weapon = _weaponHandler.CurrentWeapon;

		_playerController.OnAnimUpdate += UpdateAnim;
		_weaponHandler.OnSwitchSlot += UpdateAnimator;
	}

	private void Start()
	{
		_weaponHandler.PrimaryWeapon.OnFired += OnFired;
		_weaponHandler.SecondaryWeapon.OnFired += OnFired;
		_weaponHandler.PrimaryWeapon.OnReload += OnReload;
		_weaponHandler.SecondaryWeapon.OnReload += OnReload;

		_anim = _weaponHandler.CurrentWeapon.WeaponAnimator;
	}

	private void UpdateAnimator(Weapon currentWeapon)
	{
		Debug.Log(currentWeapon.WeaponName);
		_anim = currentWeapon.WeaponAnimator;
	}

	private void OnReload(Animator anim)
	{
		anim.Play("Reload Out Of Ammo", 0, 0f);
	}

	private void OnFired(Animator anim)
	{
		if (_movementState != EMovementStates.AimIn)
		{
			anim.Play("Fire", 0, 0f);
		}
		else
		{
			switch (_weaponHandler.CurrentWeapon.ScopeAttachment)
			{
				case EScopeAttachments.IronSights:
					anim.Play("Aim Fire", 0, 0f);
					break;
				case EScopeAttachments.Scope1:
					anim.Play("Aim Fire Scope 1", 0, 0f);
					break;
				case EScopeAttachments.Scope2:
					anim.Play("Aim Fire Scope 2", 0, 0f);
					break;
				case EScopeAttachments.Scope3:
					anim.Play("Aim Fire Scope 3", 0, 0f);
					break;
				case EScopeAttachments.Scope4:
					anim.Play("Aim Fire Scope 4", 0, 0f);
					break;
				default:
					break;
			}
		}
	}

	private void UpdateAnim(EMovementStates movementState)
	{
		_movementState = movementState;

		switch (movementState)
		{
			case EMovementStates.Idle:
				_anim.SetBool("Walk", false);
				_anim.SetBool("Run", false);
				break;
			case EMovementStates.Walk:
				_anim.SetBool("Walk", true);
				_anim.SetBool("Run", false);
				break;
			case EMovementStates.Sprint:
				_anim.SetBool("Run", true);
				_anim.SetBool("Walk", false);
				break;
			case EMovementStates.Crouch:
				break;
			case EMovementStates.AimIn:
				SetScope(_weaponHandler.CurrentWeapon.ScopeAttachment);
				break;
			case EMovementStates.AimOut:
				_anim.SetBool("Aim", false);
				_anim.SetBool("Aim Scope 1", false);
				_anim.SetBool("Aim Scope 2", false);
				_anim.SetBool("Aim Scope 3", false);
				_anim.SetBool("Aim Scope 4", false);
				break;
			default:
				break;
		}
	}

	private void SetScope(EScopeAttachments scopeAttachment)
	{
		switch (scopeAttachment)
		{
			case EScopeAttachments.IronSights:
				_anim.SetBool("Aim", true);
				break;
			case EScopeAttachments.Scope1:
				_anim.SetBool("Aim Scope 1", true);
				break;
			case EScopeAttachments.Scope2:
				_anim.SetBool("Aim Scope 2", true);
				break;
			case EScopeAttachments.Scope3:
				_anim.SetBool("Aim Scope 3", true);
				break;
			case EScopeAttachments.Scope4:
				_anim.SetBool("Aim Scope 4", true);
				break;
			default:
				break;
		}
	}
}
