using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EWeaponType
{
	AK47,
	M4A1,
	ScarL,
	GrenadeLauncher,
	Glock17,
	Baretta92,
	H3,
	H4,
	RocketLauncher,
	Shotgun,
	SMG1,
	SMG2,
	SMG3,
	SMG4,
	SMG5,
	Sniper1,
	Sniper2,
	Sniper3
}

public enum EScopeAttachments
{
	IronSights = -1,
	Scope1 = 0,
	Scope2 = 1,
	Scope3 = 2,
	Scope4 = 3
}

public class Weapon : MonoBehaviour
{
	[SerializeField] private PhotonView _photonView;

	[Header("Weapon Properties")]
	[SerializeField] private EWeaponType _weaponType;
	[SerializeField] private bool _isHandgun;
	[Space]
	[SerializeField] private string _weaponName = "Weapon";
	[SerializeField] private bool _singleFire = false;
	[SerializeField] private bool _autoReload = false;
	[SerializeField] private float _fireRate = 0.3f;
	[SerializeField] private bool _useRaycastDamage = false;
	[Space]
	[SerializeField] private bool _debugMode;

	[Header("Ammo Properties")]
	[SerializeField] private int _magazineSize = 30;
	[SerializeField] private float _reloadTime = 0.4f;
	[Space]
	[SerializeField] private Transform _bulletFireLocation;
	[SerializeField] private Transform _bulletCasingLocation;
	[Space]
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private GameObject _bulletCasing;
	[Space]
	[SerializeField] private float _bulletForce;

	[Header("Weapon Attachments")]
	[SerializeField] private EScopeAttachments _scopeAttachment;
	[SerializeField] private bool _enableSilencer;
	[SerializeField] private GameObject _silencer;
	[Space]
	[SerializeField] private List<GameObject> _scopeMesh;
	[SerializeField] private List<GameObject> _scopeRenderer;

	[Header("Weapon Effects")]
	[SerializeField] private ParticleSystem _muzzleEffect;
	[SerializeField] private Light _muzzleLight;
	[SerializeField] private float _lightDuration = 0.02f;

	private Animator _animator;

	private int _currentAmmo = 0;
	private float _lastBulletTime;
	private float _lastReloadTime;

	private bool _isFiring;

	public Animator WeaponAnimator { get => _animator; }
	public EScopeAttachments ScopeAttachment { get => _scopeAttachment; }
	public EWeaponType WeaponType { get => _weaponType; }
	public bool IsHandgun { get => _isHandgun; }
	public string WeaponName { get => _weaponName; }
	public int CurrentAmmo { get => _currentAmmo; }
	public int MagazineSize { get => _magazineSize; }

	public event System.Action<Animator> OnFired;
	public event System.Action<Animator> OnReload;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		_currentAmmo = _magazineSize;

		SetScope(_scopeAttachment);

		if (_enableSilencer)
			_silencer.SetActive(true);
		else
			_silencer.SetActive(false);
	}

	protected virtual void Update()
	{
		if (_isFiring && !_singleFire && !IsReloading() && CanFire())
			Fire();
	}

	public void BeginFire()
	{
		_isFiring = true;

		if (_singleFire && !IsReloading() && CanFire())
		{
			if(PhotonNetwork.OfflineMode)
			{
				Fire();
			}
			else
			{
				_photonView.RPC("RPC_Fire", RpcTarget.All);
			}
		}
	}

	public void EndFire()
	{
		_isFiring = false;
	}

	private void Fire()
	{
		if (_currentAmmo > 0)
		{
			OnFired?.Invoke(_animator);

			_lastBulletTime = Time.time;
			_currentAmmo--;

			SpawnBullet();
			PlayEffects();

			if (_useRaycastDamage)
			{
				if (Physics.Raycast(_bulletFireLocation.position, _bulletFireLocation.forward, out RaycastHit hit, 1000))
				{
					if (_debugMode)
					{
						Debug.DrawLine(_bulletFireLocation.position, hit.point, Color.red, 1);
						Debug.Log(hit.collider.name);
					}

					DealDamage();
				}
			}
		}
		else if (_autoReload)
		{
			Reload();
			_lastBulletTime = Time.time;
		}
		else
		{
			_lastBulletTime = Time.time;
		}
	}

	[PunRPC]
	private void RPC_Fire()
	{
		Fire();
	}

	private void DealDamage()
	{
		//var damageable = hit.collider.GetComponent<IDamageable>();
		//
		//if (damageable != null)
		//	damageable.Damage(_damage);
	}

	public void Reload()
	{
		if (_currentAmmo != _magazineSize)
		{
			OnReload?.Invoke(_animator);
			_currentAmmo = _magazineSize;
			_lastReloadTime = Time.time;
		}
	}

	private void SpawnBullet()
	{
		// Fire Bullet
		GameObject bullet = Instantiate(_bulletPrefab, _bulletFireLocation.position, _bulletFireLocation.rotation);
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _bulletForce;

		// Spawn Bullet Casings
		GameObject casing = Instantiate(_bulletCasing, _bulletCasingLocation);
		//casing.GetComponent<Rigidbody>().velocity = casing.transform.forward * _bulletCasingForce;
	}

	private bool CanFire()
	{
		return Time.time - _lastBulletTime > 60 / _fireRate;
	}

	private bool IsReloading()
	{
		return Time.time - _lastReloadTime < _reloadTime;
	}

	public void SetScope(EScopeAttachments scope)
	{
		// Turn off all scope attachments
		for (int i = 0; i < _scopeMesh.Count; i++)
		{
			_scopeMesh[i].SetActive(false);
			_scopeRenderer[i].SetActive(false);
		}

		// activate selected scope attachment
		if (scope != EScopeAttachments.IronSights)
		{
			_scopeMesh[(int)scope].SetActive(true);
			_scopeRenderer[(int)scope].SetActive(true);
		}
	}

	private void PlayEffects()
	{
		if (!_enableSilencer)
		{
			_muzzleEffect.Emit(1);
			StartCoroutine(MuzzleLight());
		}
	}

	private IEnumerator MuzzleLight()
	{
		_muzzleLight.enabled = true;
		yield return new WaitForSeconds(_lightDuration);
		_muzzleLight.enabled = false;
	}
}
