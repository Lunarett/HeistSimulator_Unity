using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class LoopBackPatrol : NPCMoveBase
	{
		[SerializeField] private List<Waypoint> _waypoints;

		int _waypointCounter = 0;
		bool isReverse = false;
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

				if (_waypointCounter == 0)
					isReverse = false;
				else if (_waypointCounter >= _waypoints.Count - 1)
					isReverse = true;

				if (!isReverse)
					_waypointCounter++;
				if (isReverse)
					_waypointCounter--;
			}

			//decides whether or not its time to move
			if (_currentWaitTime >= _waitTime)
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
