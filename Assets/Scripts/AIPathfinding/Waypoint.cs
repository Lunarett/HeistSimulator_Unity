using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class Waypoint : MonoBehaviour
	{
		[SerializeField] protected float degubDrawRadius = 1.0f;

		private Transform _user;

		[SerializeField] private bool isCover = false;
		public bool IsUsed
		{
			get => _user != null;
		}

		public bool IsCover
		{
			get => isCover;
		}
		public virtual void OnDrawGizmos()
		{
			Gizmos.color = !isCover? Color.blue : IsUsed? Color.red : Color.green;
			Gizmos.DrawWireSphere(transform.position, degubDrawRadius);
		}

		private void Start()
		{
			if(isCover)
			{
				AIHandler.Instance.AddCover(this);
			}
		}

		private void OnDestroy()
		{
			if(isCover && AIHandler.Instance != null)
				AIHandler.Instance.RemoveCover(this);
		}

		public void UseWaypoint(Transform source)
		{
			_user = source;
		}

		public void LeaveWaypoint(Transform source)
		{
			if (_user == source)
				_user = null;
			else
				Debug.Log($"{source.name} is using LeaveWaypoint() when they shouldn't.");
		}

		public bool IsUser(Transform source)
		{
			return _user == source;
		}
	}
}