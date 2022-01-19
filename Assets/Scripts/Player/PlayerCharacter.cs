using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private GameObject _tpsChar;
	[Header("View Settings")]
	[SerializeField] private Camera _camera;
	[SerializeField] private Camera _gunCamera;
	[SerializeField] private Transform _playerArms;
	[SerializeField] private Transform _playerTransform;
	[Range(0, 2)]
	[SerializeField] private float _mouseSensitivity;

	[Space]
	[Space]
	[Header("Movement Settings")]
	[SerializeField] private float _jumpSpeed = 15;
	[SerializeField] private float _gravity = 0.1f;
	[SerializeField] private float _walkSpeed = 4;
	[SerializeField] private float _sprintSpeed = 2;
	[SerializeField] private float _crouchSpeed = 1;
	[Space]
	[Space]
	[Header("Animation References")]
	[SerializeField] private Animator[] _camAnimator;
	[SerializeField] private GameObject _crossHair;


	[Range(0, 2)]
	[SerializeField] private float _aimTime = 0.085f;
	[SerializeField] private float _currentAimTime = 3f;

	public List<OldWeapon> WeaponList = new List<OldWeapon>();

	[HideInInspector] public OldWeapon CurrentWeapon { get; private set; }
	[HideInInspector] public bool IsHolstered;

	public int WeaponIndex { get; private set; }
	private float _xAxisClamp = 0;

	private Vector3 _moveDirection;
	private float _moveSpeed;
	private float _lastAimTime;

	private bool _isAiming;
	private bool _isCrouched;
	private bool _isSprinting;
	
	private Animator _weaponAnimator;
	private CharacterController _controller;
	private PhotonView _photonView;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		_controller = GetComponent<CharacterController>();
		ChangeWeapon();

		_photonView = GetComponent<PhotonView>();

		foreach (OldWeapon weapon in WeaponList)
		{
			weapon.Fired += OnFired;
		}
	}

	private void Start()
	{
		if (!_photonView.IsMine)
		{
			Destroy(_camera.gameObject);
			Destroy(_gunCamera.gameObject);
			_playerArms.gameObject.SetActive(false);

			_tpsChar.SetActive(true);
			return;
		}
		else
		{
			_tpsChar.SetActive(false);
		}
	}

	private void OnFired()
	{
		if (_photonView.IsMine)
		{
			if (!_isAiming)
			{
				_weaponAnimator.Play("Fire", 0, 0f);
			}
			else
			{
				_weaponAnimator.Play("Aim Fire", 0, 0f);
			}
		}
	}

	private void Update()
	{
		if (Input.mouseScrollDelta.y != 0)
			ChangeWeapon();

		Move();
		RotateCamera();

		if (Input.GetKeyDown(Keybindings.Hide_ShowWeapon))
			HolsterWeapon();

		if (!IsHolstered)
		{
			Aim();
			Shoot();

			if (!_isSprinting && Input.GetKeyDown(Keybindings.ReloadWeapon))
				Reload();
		}
	}

	private void Move()
	{
		if (_photonView.IsMine)
		{
			float moveX = Input.GetAxis("Horizontal");
			float moveZ = Input.GetAxis("Vertical");

			if (moveX != 0 || moveZ != 0)
			{
				_weaponAnimator.SetBool("Walk", true);
			}
			else
			{
				_weaponAnimator.SetBool("Walk", false);
			}


			if (_controller.isGrounded)
			{
				_moveDirection = new Vector3(moveX, 0, moveZ);
				_moveDirection = transform.TransformDirection(_moveDirection);

				_isCrouched = false;

				// Sprint & Crouch
				if (!_isAiming && Input.GetKey(Keybindings.PlayerSprint) && moveZ == 1)
				{
					_isSprinting = true;

					_moveSpeed = _sprintSpeed;

					_weaponAnimator.SetBool("Run", true);
				}
				else if (!_isSprinting && Input.GetKey(Keybindings.PlayerCrouch))
				{
					_controller.height = 1.5f;
					_moveSpeed = _crouchSpeed;
					_isCrouched = true;
				}
				else
				{
					_isSprinting = false;

					_moveSpeed = _walkSpeed;

					_weaponAnimator.SetBool("Run", false);
				}

				if (!_isCrouched)
				{
					_controller.height = 2;
				}

				_moveDirection *= _moveSpeed;

				if (Input.GetKeyDown(Keybindings.PlayerJump))
				{
					_moveDirection.y += _jumpSpeed;
				}
			}

			_moveDirection.y -= _gravity * Time.deltaTime;
			_controller.Move(_moveDirection * Time.deltaTime);
		}
	}

	private void RotateCamera()
	{
		if (_photonView.IsMine)
		{
			if (Time.timeScale == 0)
			{
				return;
			}

			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");

			float rotAmountX = mouseX * _mouseSensitivity;
			float rotAmountY = mouseY * _mouseSensitivity;

			_xAxisClamp -= rotAmountY;

			Vector3 rotPlayerArms = _playerArms.rotation.eulerAngles;
			Vector3 rotPlayer = transform.rotation.eulerAngles;

			rotPlayerArms.x -= rotAmountY;
			rotPlayerArms.z = 0;
			rotPlayer.y += rotAmountX;

			if (_xAxisClamp > 90)
			{
				_xAxisClamp = 90;
				rotPlayerArms.x = 90;
			}
			else if (_xAxisClamp < -90)
			{
				_xAxisClamp = -90;
				rotPlayerArms.x = 270;
			}

			_playerArms.rotation = Quaternion.Euler(rotPlayerArms);
			transform.rotation = Quaternion.Euler(rotPlayer);
		}
	}

	private void Aim()
	{
		if (_photonView.IsMine)
		{
			if ((!_isSprinting && Input.GetMouseButton(1) || Input.GetAxis("Aim") > 0) && _currentAimTime >= _aimTime)
			{
				_weaponAnimator.SetBool("Aim", true);

				_isAiming = true;
				_crossHair.SetActive(false);

				for (int i = 0; i < _camAnimator.Length; i++)
					_camAnimator[i].SetBool("Aim", true);
			}
			else if (Input.GetMouseButtonUp(1) && _isAiming)
			{
				_currentAimTime = 0;
				_isAiming = false;

				_weaponAnimator.SetBool("Aim", false);
				_crossHair.SetActive(true);


				_isAiming = false;

				for (int i = 0; i < _camAnimator.Length; i++)
					_camAnimator[i].SetBool("Aim", false);
			}
			else
				_currentAimTime += 2.5f * Time.deltaTime;
		}
	}

	private void Shoot()
	{
		if (_photonView)
		{
			if (!_isSprinting && Input.GetMouseButtonDown(0))
			{
				CurrentWeapon.BeginFire();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				CurrentWeapon.StopFire();
			}
		}
	}

	private void Reload()
	{
		if (_photonView.IsMine)
		{
			if (CurrentWeapon.CurrentAmmo != CurrentWeapon.MagazineSize)
			{
				if (CurrentWeapon.isReloading())
					return;
				CurrentWeapon.Reload();

				if (CurrentWeapon.CurrentAmmo > 0)
				{
					_weaponAnimator.Play("Reload Ammo Left");
				}
				else
				{
					_weaponAnimator.Play("Reload Out Of Ammo");
				}
			}
		}
	}

	private void HolsterWeapon()
	{
		if (_photonView.IsMine)
		{
			if (IsHolstered)
			{
				_weaponAnimator.SetBool("Holster", false);
				IsHolstered = false;
			}
			else
			{
				_weaponAnimator.SetBool("Holster", true);
				IsHolstered = true;
			}
		}
	}

	private void ChangeWeapon()
	{
		if (_photonView.IsMine)
		{
			IsHolstered = false;

			if (CurrentWeapon != null)
				CurrentWeapon.StopFire();

			WeaponIndex = Mathf.Clamp(WeaponIndex + Mathf.RoundToInt(Input.mouseScrollDelta.y), 0, WeaponList.Count - 1);

			for (int i = 0; i < WeaponList.Count; i++)
			{
				if (i != WeaponIndex)
				{
					WeaponList[i].gameObject.SetActive(false);
				}
				else
				{
					WeaponList[i].gameObject.SetActive(true);
				}
			}

			CurrentWeapon = WeaponList[WeaponIndex];
			_weaponAnimator = CurrentWeapon.GetComponent<Animator>();
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (_photonView.IsMine)
		{
			if (hit.rigidbody == null || hit.rigidbody.isKinematic) { return; }

			if (hit.moveDirection.y < -0.3) { return; }

			var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

			hit.rigidbody.velocity = pushDir * 2;
		}
	}
}
