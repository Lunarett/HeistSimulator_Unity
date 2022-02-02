using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationHandler : MonoBehaviour
{
	[SerializeField] private Animator _tpsAnimator;

	private PlayerController _playerController;
	private PlayerWeaponHandler _weaponHandler;
	private Animator _anim;
	private PhotonView _photonView;

	private EMovementStates _movementState;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		_weaponHandler = GetComponent<PlayerWeaponHandler>();
		_photonView = GetComponent<PhotonView>();

		_playerController.OnAnimUpdate += MovementAnimation;
		_weaponHandler.OnSwitchSlot += UpdateAnimator;
	}

	private void Start()
	{
		if (_photonView.IsMine)
		{
			_weaponHandler.PrimaryWeapon.OnFired += OnFired;
			_weaponHandler.SecondaryWeapon.OnFired += OnFired;
			_weaponHandler.PrimaryWeapon.OnReload += OnReload;
			_weaponHandler.SecondaryWeapon.OnReload += OnReload;

			_playerController.OnKnifeAttack += KnifeAttack;
			_playerController.OnGrenadeThrow += GrenadeThrow;

			_anim = _weaponHandler.CurrentWeapon.WeaponAnimator;
		}
	}

	private void GrenadeThrow()
	{
		_anim.Play("GrenadeThrow", 0, 0);
	}

	private void KnifeAttack()
	{
		_anim.Play("Knife Attack 2", 0, 0);
	}

	private void Update()
	{
		if (_photonView.IsMine)
		{
			TPSMovement();
		}
	}

	private void UpdateAnimator(Weapon currentWeapon)
	{
		_anim = currentWeapon.WeaponAnimator;
	}

	private void TPSMovement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		_tpsAnimator.SetBool("IsHandgun", _weaponHandler.CurrentWeapon.IsHandgun);

		_tpsAnimator.SetFloat("Vertical", vertical, 0, Time.deltaTime);
		_tpsAnimator.SetFloat("Horizontal", horizontal, 0, Time.deltaTime);
	}

	private void OnReload(Animator anim)
	{
		if (_photonView.IsMine)
		{
			anim.Play("Reload Out Of Ammo", 0, 0f);
		}
		else
		{
			if (_weaponHandler.CurrentWeapon.IsHandgun)
				_tpsAnimator.Play("Reload_Handgun", 1, 0.0f);
			else
				_tpsAnimator.Play("Reload_Rifle", 1, 0.0f);
		}
	}

	private void OnFired(Animator anim)
	{
		if (_photonView.IsMine)
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
		else
		{
			if (_weaponHandler.CurrentWeapon.IsHandgun)
				_tpsAnimator.Play("Fire_Handgun", 1, 0.0f);
			else
				_tpsAnimator.Play("Fire_Rifle", 1, 0.0f);
		}
	}

	private void MovementAnimation(EMovementStates movementState)
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
