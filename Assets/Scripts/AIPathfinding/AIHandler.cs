using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class AIHandler : MonoBehaviour
	{
		[SerializeField] private GameObject _playerTransform;

		private List<Waypoint> _coverPoints;
		private static AIHandler _instance;
		private List<NPCBase> _enemiesList = new List<NPCBase>();

		public event Action Aggro;

		public int KillCount { get; private set; }

		public static AIHandler Instance
		{
			get => _instance;
		}

		public GameObject PlayerTransform
		{
			get => _playerTransform;
		}

		public List<Waypoint> CoverPoints
		{
			get => _coverPoints;
		}
		private void Awake()
		{
			_coverPoints = new List<Waypoint>();

			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				Debug.LogError("Second instance of AIHandler!");
				Destroy(this);
			}
		}

		private void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		public void AggroAll()
		{
			Aggro?.Invoke();
		}

		public void AddCover(Waypoint newWaypoint)
		{
			_coverPoints.Add(newWaypoint);
		}

		public void RemoveCover(Waypoint oldWaypoint)
		{
			_coverPoints.Remove(oldWaypoint);
		}

		public void AddEnemy(NPCBase enemy)
		{
			_enemiesList.Add(enemy);
			enemy.Death += OnDeath;
		}

		public void RemoveEnemy(NPCBase enemy)
		{
			_enemiesList.Remove(enemy);
			enemy.Death -= OnDeath;
		}

		private void OnDeath()
		{
			KillCount++;
		}
	}
}