using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AIPathfinding
{
	public class LoopPatrol : NPCBase
	{
		[SerializeField] private List<Waypoint> _waypoints;
		[SerializeField] private Waypoint _closestCover;
		[SerializeField] private Waypoint _coverOverride;
		[SerializeField] private Transform _spine;

		private PlayerCharacter _player;

		int _waypointCounter = 0;
	
		private float _currentWaitTime = 4.0f;
		[SerializeField] private float _waitTime = 4.0f;

		protected override void Start()
		{
			base.Start();

			_player = AIHandler.Instance.PlayerTransform.GetComponent<PlayerCharacter>();
		}

		protected override void Update()
		{
			base.Update();

			if ((CanSeePlayer() || _currentState == NPCStates.Aggro) && !_player.IsHolstered && _currentState != NPCStates.Commando)
			{
				if (_currentState != NPCStates.Aggro)
				{
					AIHandler.Instance.AggroAll();
				}
				
				if (!AtBestWaypoint())
				{
					if (_closestCover != null)
					{
						_closestCover.LeaveWaypoint(transform);
					}
					FindBestCover();
				}
			}
			else
			{
				_currentState = NPCStates.Alert;

				//decides whether or not its time to move
				if (TargetReached())
				{
					if (_currentWaitTime >= _waitTime && _waypoints.Count > 0)
					{
						FindNextWaypoint();
					}
					else
					{
						_currentWaitTime += 1 * Time.deltaTime;
					}
				}
			}
		}

		private void LateUpdate()
		{
			if (_currentState == NPCStates.Aggro)
			{
				float dir = Vector3.Angle(transform.forward, GetPlayerTransform().position - _spine.position);

				if(transform.position.y < GetPlayerTransform().position.y)
				{
					dir = -dir;
				}
				
				_spine.localEulerAngles = new Vector3(_spine.localEulerAngles.x + 180, 0, 25 + dir);
			}
		}

		private void FindNextWaypoint()
		{
			if (TargetReached())
			{
				//resets timer 
				_currentWaitTime = 0;

				//finds the next destination
				_targetPosition = _waypoints[_waypointCounter].transform.position;

				//increments counter with overflow exception
				_waypointCounter = (_waypointCounter + 1) % _waypoints.Count;
			}
		}

		private void FindBestCover()
		{
			_closestCover = null;
			if (_coverOverride == null)
			{
				for (int i = 0; i < GetWaypoints().Count; i++)
				{
				Waypoint current = GetWaypoints()[i];
				
					if (_closestCover == null)
					{
						if (!current.IsUsed)
							_closestCover = current;
					}
					else if (PlayerDistanceTo(current.transform) < PlayerDistanceTo(_closestCover.transform) && !current.IsUsed)
					{
						_closestCover = current;
					}
				}
			}
			else
			{
				_closestCover = _coverOverride;
			}

			if (_closestCover != null)
			{
				_targetPosition = _closestCover.transform.position;
				_closestCover.UseWaypoint(transform);
			}
			else
			{
				Debug.Log("Can't find cover.");
			}
		}

		private bool AtBestWaypoint()
		{
			Waypoint closestCover;
			closestCover = GetWaypoints()[0];

			bool isBest = false;

			for (int i = 1; i < GetWaypoints().Count; i++)
			{
				Waypoint current = GetWaypoints()[i];
				if (PlayerDistanceTo(current.transform) < PlayerDistanceTo(closestCover.transform))
				{
					if(current.IsUser(transform))
					{
						closestCover = current;
						isBest = true;
					}
					else
					{
						if (current.IsUsed)
						{
							closestCover = GetWaypoints()[i];
							isBest = false;
						}
					}
				}
			}

			return isBest;
		}

		private List<Waypoint> GetWaypoints()
		{
			return AIHandler.Instance.CoverPoints;
		}
	}
}