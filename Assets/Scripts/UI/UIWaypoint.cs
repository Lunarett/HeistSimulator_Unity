using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWaypoint : MonoBehaviour
{
	[SerializeField] private Sprite _worldIcon;
	[SerializeField] private Sprite _compassIcon;
	[SerializeField] private Transform _target;
	[SerializeField] private Vector3 _offset;
	[SerializeField] private GameObject _imagePrefab;
	[SerializeField] private PlayerDeath _playerDeath;
	[SerializeField] private float _distance = 30;
	[SerializeField] private AnimationCurve _animCurve;

	public Sprite CompassIcon { get => _compassIcon; }
	private Image _img;
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;

		_img = Instantiate(_imagePrefab).GetComponentInChildren<Image>();

		_img.sprite = _worldIcon;
	}

	private void Update()
	{
		if (!_playerDeath.IsDead)
		{
			Vector2 pos = _camera.WorldToScreenPoint(transform.position + _offset);

			if (Vector3.Dot(transform.position - _target.position, _target.forward) < 0)
			{
				_img.enabled = false;
			}
			else
			{
				_img.enabled = true;
			}

			_img.transform.position = pos;

			float dif = Vector3.Distance(_target.position, transform.position);
			Color color = new Color(_img.color.r, _img.color.g, _img.color.b, _animCurve.Evaluate(dif));
			_img.color = color;
		}
	}
}
