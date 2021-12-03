using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class BackAndForthPatrol : NPCMoveBase
	{
		[SerializeField] private List<Waypoint> _waypoints;

		int _waypointCounter = 0;

		private float _currentWaitTime = 4.0f;
		[SerializeField] private float _waitTime = 4.0f;

		protected override float PlayerDistance(Transform target)
		{
			throw new NotImplementedException();
		}

		protected override void SetDestination()
		{
			if (((_destination == null) || (_destination.position == transform.position)))
			{
				//resets timer 
				_currentWaitTime = 0;

				//finds the next destination
				_destination = _waypoints[_waypointCounter].transform;
				_destination.position = _waypoints[_waypointCounter].transform.position;

				//increments counter with overflow exception
				_waypointCounter = (_waypointCounter + 1) % _waypoints.Count;
			}

			//decides whether or not its time to move
			if(_currentWaitTime >= _waitTime)
			{
				_navMeshAgent.SetDestination(_destination.position);
			}
			else
			{
				_currentWaitTime += 1 * Time.deltaTime;
			}
		}
	}
}