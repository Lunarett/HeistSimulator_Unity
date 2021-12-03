using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Weapon Properties")]
	public string Name;
	public Sprite WeaponIcon;
	[SerializeField] private bool _isSingleFire;
	[SerializeField] private float _fireRate;

	[Space]
	[Header("Muzzle Settings")]
	[SerializeField] private ParticleSystem _muzzleParticle;
	[SerializeField] private Light _muzzleLight;
	[SerializeField] private Transform _muzzleLocation;

	[Space]
	[Header("Reload Settings")]
	[SerializeField] private float _reloadTime;
	[SerializeField] private int _magazineSize = 31;

	[Space]
	[Header("Bullet Settings")]
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private Transform _bulletSpawnPoint;
	[SerializeField] private float _bulletForce;
	[SerializeField] private float _raycastDistance = 50;
	[SerializeField] private float _damage = 10;
	[SerializeField] private GameObject _fleshParticlePrefab;

	[Space] [Space]
	[Header("Weapon Sway Settings")]
	[SerializeField] private bool weaponSway;
	[SerializeField] private float swayAmount = 0.02f;
	[SerializeField] private float maxSwayAmount = 0.06f;
	[SerializeField] private float swaySmoothValue = 4.0f;

	[Space] [Space]
	[Header("FMOD Events")] [FMODUnity.EventRef]
	[SerializeField] private string _shotFEvent;
	[FMODUnity.EventRef]
	[SerializeField] private string _noAmmoFEvent;
	[FMODUnity.EventRef]
	[SerializeField] private string _reloadFEvent;
	[FMODUnity.EventRef]
	[SerializeField] private string _chargeFEvent;
	[FMODUnity.EventRef]
	[SerializeField] private string _playerDamageFEvent;

	[SerializeField] private CameraShake _cameraShake;

	[Space]
	[Space]
	[SerializeField] private bool _debugMode;

	private int _currentAmmo = 0;
	private Vector3 initialSwayPosition;
	private float _lastBulletTime;
	private float _lastReloadTime;
	private bool _isFiring;
	private float _timer = 1;

	public event System.Action Fired;
	public int CurrentAmmo { get => _currentAmmo; }
	public int MagazineSize { get => _magazineSize; }


	private void Start()
	{
		initialSwayPosition = transform.localPosition;

		_currentAmmo = _magazineSize;

		_muzzleLight.enabled = false;

		FMODUnity.RuntimeManager.PlayOneShotAttached(_chargeFEvent, gameObject);
	}
	
	protected virtual void Update()
	{
		if (_isFiring && !_isSingleFire && !isReloading() && CanFire())
			Fire();
	}
	
	private void LateUpdate()
	{
		WeaponSway();
	}

	public void BeginFire()
	{
		_isFiring = true;
		
		if(_isSingleFire && !isReloading() && CanFire())
		{
			Fire();
		}
	}

	public void StopFire()
	{
		_isFiring = false;
	}

	protected void Fire()
	{
		if (_currentAmmo > 0)
		{
			_currentAmmo -= 1;

			FMODUnity.RuntimeManager.PlayOneShotAttached(_shotFEvent, gameObject);

			_lastBulletTime = Time.time;

			Fired?.Invoke();

			// Spawn Bullet
			GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _bulletForce;
			
			if(Physics.Raycast(_bulletSpawnPoint.position, _bulletSpawnPoint.forward, out RaycastHit hit, _raycastDistance))
			{
				if(_debugMode)
				{
					Debug.DrawLine(_bulletSpawnPoint.position, hit.point, Color.red, 1);
					Debug.Log(hit.collider.name);
				}

				if(hit.collider.CompareTag("Player"))
					FMODUnity.RuntimeManager.PlayOneShotAttached(_playerDamageFEvent, gameObject);

				if (_fleshParticlePrefab != null && hit.collider.CompareTag("Enemy"))
					Instantiate(_fleshParticlePrefab, hit.point, Quaternion.identity, hit.collider.transform);

				var damageable = hit.collider.GetComponent<IDamageable>();

				if (damageable != null)
					damageable.Damage(_damage);
			}

			if (_cameraShake != null)
				_cameraShake.ShakeCamera(0.08f, 0.0015f);

			_muzzleParticle.Emit(1);
			StartCoroutine(MuzzleFlash());
		}
		else
		{
			FMODUnity.RuntimeManager.PlayOneShotAttached(_noAmmoFEvent, gameObject);
			_lastBulletTime = Time.time;
		}
	}

	System.Collections.IEnumerator MuzzleFlash()
	{
		_muzzleLight.enabled = true;

		yield return new WaitForSeconds(0.1f);
		_muzzleLight.enabled = false;
	}

	private bool CanFire()
	{
		return Time.time - _lastBulletTime > 60 / _fireRate;
	}

	private void WeaponSway()
	{
		if (weaponSway) 
		{
			float movementX = Mathf.Clamp (-Input.GetAxis ("Mouse X") * swayAmount, -maxSwayAmount, maxSwayAmount);
			float movementY = Mathf.Clamp (-Input.GetAxis ("Mouse Y") * swayAmount, -maxSwayAmount, maxSwayAmount);
			
			Vector3 finalSwayPosition = new Vector3(movementX, movementY, 0);
			
			transform.localPosition = Vector3.Lerp (transform.localPosition, finalSwayPosition + initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}

	public void Reload()
	{
		if(_currentAmmo != _magazineSize)
		{
			_currentAmmo = _magazineSize;
			FMODUnity.RuntimeManager.PlayOneShotAttached(_reloadFEvent, gameObject);

			_lastReloadTime = Time.time;
		}
	}

	public bool isReloading()
	{
		return Time.time - _lastReloadTime < _reloadTime;
	}
}
