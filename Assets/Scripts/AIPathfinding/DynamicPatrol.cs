using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AIPathfinding
{
	public class DynamicPatrol : MonoBehaviour
	{
		[SerializeField] bool _patrolWaiting;
		[SerializeField] float _totalWaitTime = 3f;
		[SerializeField] float _switchProbability = 0.2f;

		NavMeshAgent _navMeshAgent;
		ConnectedWaypoint _currentWaypoint;
		ConnectedWaypoint _previousWaypoint;

		bool _isTravelling;
		bool _isWaiting;
		float _waitTimer;
		int _waypointsVisited;

		private void Start()
		{
			_navMeshAgent = this.GetComponent<NavMeshAgent>();

			if(_navMeshAgent == null)
			{
				Debug.LogError("No Nav Mesh Agent attached.");
			}
			else
			{
				if(_currentWaypoint == null)
				{
					GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

					if(allWaypoints.Length > 0)
					{
						while (_currentWaypoint == null)
						{
							int random = UnityEngine.Random.Range(0, allWaypoints.Length);
							ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

							if (startingWaypoint != null)
							{
								_currentWaypoint = startingWaypoint;
							}
						}
					}
					else
					{
						Debug.LogError("No usable Waypoints.");
					}
				}
				SetDestination();
			}
		}

		private void Update()
		{
			if(_isTravelling && _navMeshAgent.remainingDistance <= 1.0f)
			{
				_isTravelling = false;
				_waypointsVisited++;

				if(_patrolWaiting)
				{
					_isWaiting = true;
					_waitTimer = 0f;
				}
				else
				{
					SetDestination();
				}
			}

			if(_isWaiting)
			{
				_waitTimer += Time.deltaTime;

				if(_waitTimer >= _totalWaitTime)
				{
					_isWaiting = false;

					SetDestination();
				}
			}
		}

		private void SetDestination()
		{
			if (_waypointsVisited > 0)
			{
				ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
				_previousWaypoint = _currentWaypoint;
				_currentWaypoint = nextWaypoint;
			}

			Vector3 targetVector = _currentWaypoint.transform.position;
			_navMeshAgent.destination = targetVector;
			_isTravelling = true;
		}
	}
}