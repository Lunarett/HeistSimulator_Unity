using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
	private PlayerController _playerController;
	private PlayerWeaponHandler _weaponHandler;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		_weaponHandler = GetComponent<PlayerWeaponHandler>();

		_playerController.OnAnimUpdate += UpdateAnim;
	}

	private void UpdateAnim(EMovementStates movementState)
	{
		Animator anim = _weaponHandler.GetCurrentWeapon().GetComponent<Animator>();

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
			case EMovementStates.Aim:
				break;
			default:
				break;
		}
	}
}
