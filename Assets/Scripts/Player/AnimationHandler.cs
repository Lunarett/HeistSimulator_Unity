using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
	private PlayerController _playerController;
	private PlayerWeaponHandler _weaponHandler;
	private Weapon _weapon;
	private Animator anim;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		_weaponHandler = GetComponent<PlayerWeaponHandler>();
		_weapon = _weaponHandler.GetCurrentWeapon();

		_playerController.OnAnimUpdate += UpdateAnim;
	}

	private void Start()
	{
		_weaponHandler.GetCurrentWeapon().OnFired += OnFired;
		_weaponHandler.GetCurrentWeapon().OnReload += OnReload;

		anim = _weaponHandler.GetCurrentWeapon().GetAnimator();
	}

	private void OnReload()
	{
		Animator anim = _weaponHandler.GetCurrentWeapon().GetAnimator();
		anim.Play("Reload Out Of Ammo", 0, 0f);
	}

	private void OnFired()
	{
		Animator anim = _weaponHandler.GetCurrentWeapon().GetAnimator();
		anim.Play("Fire", 0, 0f);
	}

	private void UpdateAnim(EMovementStates movementState)
	{
		switch (movementState)
		{
			case EMovementStates.Idle:
				anim.SetBool("Walk", false);
				anim.SetBool("Run", false);
				break;
			case EMovementStates.Walk:
				anim.SetBool("Walk", true);
				anim.SetBool("Run", false);
				break;
			case EMovementStates.Sprint:
				anim.SetBool("Run", true);
				anim.SetBool("Walk", false);
				break;
			case EMovementStates.Crouch:
				break;
			case EMovementStates.AimIn:
				//anim.SetBool("Walk", false);
				SetScope(anim, _weaponHandler.GetCurrentWeapon().GetScopeAttachment());
				break;
			case EMovementStates.AimOut:
				anim.SetBool("Aim", false);
				anim.SetBool("Aim Scope 1", false);
				anim.SetBool("Aim Scope 2", false);
				anim.SetBool("Aim Scope 3", false);
				anim.SetBool("Aim Scope 4", false);
				break;
			default:
				break;
		}
	}

	private void SetScope(Animator anim, EScopeAttachments scopeAttachment)
	{

		switch (scopeAttachment)
		{
			case EScopeAttachments.IronSights:
				anim.SetBool("Aim", true);
				break;
			case EScopeAttachments.Scope1:
				anim.SetBool("Aim Scope 1", true);
				break;
			case EScopeAttachments.Scope2:
				anim.SetBool("Aim Scope 2", true);
				break;
			case EScopeAttachments.Scope3:
				anim.SetBool("Aim Scope 3", true);
				break;
			case EScopeAttachments.Scope4:
				anim.SetBool("Aim Scope 4", true);
				break;
			default:
				break;
		}
	}
}
