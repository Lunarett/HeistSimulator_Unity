using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum EMovementStates
{
	Idle,
	Walk,
	Sprint,
	Crouch,
	AimIn,
	AimOut
}

public class PlayerController : MonoBehaviour
{
	[Header("Mesh Properties")]
	[SerializeField] private GameObject _fps;
	[SerializeField] private GameObject _tps;
	[Space]
	[SerializeField] private Image _crosshair;

	[Header("Movement Properties")]
	[SerializeField] private float _walkSpeed = 3.0f;
	[SerializeField] private float _sprintSpeed = 6.0f;
	[SerializeField] private float _crouchSpeed = 1.5f;
	[SerializeField] private float _jumpForce = 3.0f;
	[SerializeField] private bool _toggleSprint = false;
	
	[Header("Camera Properties")]
	[SerializeField] private Camera _camera;
	[Space]
	[SerializeField] private float _sensitivity = 100.0f;
	[SerializeField] private float _viewingRangeMin = -90.0f;
	[SerializeField] private float _viewingRangeMax = 90.0f;

	[Header("Sway Settings")]
	[SerializeField] private Transform _swayObject;
	[Space]
	[SerializeField] private float _locationSwayAmount = 0.02f;
	[SerializeField] private float _locationMinSway = -0.06f;
	[SerializeField] private float _locationMaxSway = 0.06f;
	[Space]
	[SerializeField] private float _locationSwaySmooth = 4.0f;

	private EMovementStates _movementState;

	private Vector3 _velocity = Vector3.zero;
	private Vector3 _initSwayPos;
	private Vector3 _initSwayRot;
	
	private const float _gravity = 9.81f;
	private float _currentSpeed = 0.0f;
	private float _yaw = 0.0f;
	private float _pitch = 0.0f;

	private PhotonView _photonView;
	private PlayerWeaponHandler _weaponHandler;
	private CharacterController _characterController;

	public EMovementStates MovementState;

	public event System.Action<EMovementStates> OnAnimUpdate;

	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
		_weaponHandler = GetComponent<PlayerWeaponHandler>();
		_photonView = GetComponent<PhotonView>();
	}

	private void Start()
	{

		if(_photonView.IsMine)
		{
			_fps.SetActive(true);
			_tps.SetActive(false);

			Cursor.lockState = CursorLockMode.Locked;
			
			_initSwayPos = _swayObject.transform.localPosition;
			_initSwayRot = _swayObject.transform.eulerAngles;

			_currentSpeed = _walkSpeed;
			_pitch = _camera.transform.rotation.y;
			_yaw = _camera.transform.rotation.x;
		}
		else
		{
			_fps.SetActive(false);
			_tps.SetActive(true);
		}
	}

	private void Update()
	{
		if(_photonView.IsMine)
		{
			// Jump when Key is pressed
			if (Input.GetButtonDown("Jump") && _characterController.isGrounded) Jump();

			// If player is Idling
			if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
			{
				UpdateMovementState(EMovementStates.Idle);
			}
			else if(_movementState != EMovementStates.AimIn)
			{
				UpdateMovementState(EMovementStates.Walk);
			}

			Aim();
			Reload();
			Sprint();
			
			Movement();
			Look();
			ShootWeapon();
			Debug.Log(_movementState.ToString());

		}
	}

	private void LateUpdate()
	{
		if(_photonView.IsMine)
		{
			Sway();
		}
	}

	private void Movement()
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");

		Vector3 move = transform.right * moveX + transform.forward * moveZ;
		_characterController.Move(move * _currentSpeed * Time.deltaTime);

		if (_characterController.isGrounded && _velocity.y < 0) _velocity.y = 0.2f;

		_velocity.y -= _gravity * Time.deltaTime;
		_characterController.Move(_velocity * Time.deltaTime);
	}

	private void Jump()
	{
		_velocity.y = _jumpForce;
	}

	private void Look()
	{
		float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

		_yaw -= -mouseX;
		_pitch -= mouseY;

		_pitch = Mathf.Clamp(_pitch, _viewingRangeMin, _viewingRangeMax);

		_fps.transform.localRotation = Quaternion.Euler(_pitch, 0.0f, 0.0f);
		transform.localRotation = Quaternion.Euler(0.0f, _yaw, 0.0f);
	}

	private void Sprint()
	{
		if(_movementState != EMovementStates.Idle && _movementState != EMovementStates.Crouch && _movementState != EMovementStates.AimIn)
		{
			if (Input.GetButton("Sprint"))
			{
				_currentSpeed = _sprintSpeed;
				UpdateMovementState(EMovementStates.Sprint);
			}
			else
			{
				_currentSpeed = _walkSpeed;
				UpdateMovementState(EMovementStates.Walk);
			}
		}
	}

	private void Sway()
	{
		float locMovX = Mathf.Clamp(-Input.GetAxis("Mouse X") * _locationSwayAmount, _locationMinSway, _locationMaxSway);
		float locMovY = Mathf.Clamp(-Input.GetAxis("Mouse Y") * _locationSwayAmount, _locationMinSway, _locationMaxSway);

		//float RotMovX = Mathf.Clamp(-Input.GetAxis("Mouse X") * _rotationSwayAmount, _rotationMinSway, _rotationMaxSway);
		//float RotMovY = Mathf.Clamp(-Input.GetAxis("Mouse Y") * _rotationSwayAmount, _rotationMinSway, _rotationMaxSway);

		Vector3 newLocation = new Vector3(locMovX, locMovY, 0);

		_swayObject.localPosition = Vector3.Lerp(_swayObject.localPosition, newLocation + _initSwayPos, Time.deltaTime * _locationSwaySmooth);
		//_swayObject.localEulerAngles = Vector3.Lerp(_swayObject.localEulerAngles, newRotation + _initSwayRot, Time.deltaTime * _rotationSwaySmooth);
	}

	private void ShootWeapon()
	{
		if (_movementState != EMovementStates.Sprint && Input.GetMouseButtonDown(0))
		{
			_weaponHandler.GetCurrentWeapon().BeginFire();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			_weaponHandler.GetCurrentWeapon().EndFire();
		}
	}

	private void Aim()
	{
		if(_movementState != EMovementStates.Sprint)
		{
			if(Input.GetMouseButton(1))
			{
				UpdateMovementState(EMovementStates.AimIn);
				_crosshair.gameObject.SetActive(false);
			}
			else if(Input.GetMouseButtonUp(1))
			{
				UpdateMovementState(EMovementStates.AimOut);
				_crosshair.gameObject.SetActive(true);
			}
		}
	}

	private void Reload()
	{
		if (Input.GetKey(KeyCode.R))
		{
			_weaponHandler.GetCurrentWeapon().Reload();
		}
	}

	private void UpdateMovementState(EMovementStates movementState)
	{
		_movementState = movementState;
		OnAnimUpdate?.Invoke(_movementState);
	}
}