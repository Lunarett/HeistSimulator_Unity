using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	[SerializeField] private GameObject _wayPointIcon;
	[SerializeField] private RectTransform _northLayer;

	[SerializeField] private Transform _player;
	[SerializeField] private List<UIWaypoint> _waypoints = new List<UIWaypoint>();

	private Vector3 _northDirection;
	private List<CompassWaypoint> _compassWaypoints = new List<CompassWaypoint>();

	private void Start()
	{
		foreach (UIWaypoint wp in _waypoints)
		{
			CompassWaypoint compWP = Instantiate(_wayPointIcon, transform).GetComponent<CompassWaypoint>();

			_compassWaypoints.Add(compWP);

			compWP.SetDefaultSprite(wp.CompassIcon);
		}
	}

	private void Update()
	{
		ChangeNorthDirection();

		for (int i = 0; i < _waypoints.Count; i++)
		{
			ChangeWaypointDirection(i);	
		}
	}

	private void ChangeNorthDirection()
	{
		_northDirection.z = _player.eulerAngles.y;
		_northLayer.localEulerAngles = _northDirection;
	}

	private void ChangeWaypointDirection(int index)
	{
		Vector3 dir = _player.position - _waypoints[index].transform.position;
		_compassWaypoints[index].UpdateDistance(dir.magnitude);

		dir.y = 0;

		Vector3 rotation = new Vector3(0, 0, Vector3.Angle(dir, _player.forward));

		if (Vector3.Dot(Vector3.Cross(dir, Vector3.up), _player.forward) > 0)
		{
			rotation = -rotation;
		}

		_compassWaypoints[index].transform.eulerAngles = rotation;

		_compassWaypoints[index].FixLocalRotation();
	}
}
