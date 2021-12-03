using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class ConnectedWaypoint : Waypoint
	{
		[SerializeField]
		protected float _connectiviyRadius = 50f;

		List<ConnectedWaypoint> _connections;

		private void Start()
		{
			GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

			_connections = new List<ConnectedWaypoint>();

			for(int i = 0; i < allWaypoints.Length; i++)
			{
				ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

				if(nextWaypoint != null)
				{
					if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectiviyRadius && nextWaypoint != this)
					{
						_connections.Add(nextWaypoint);
					}
				}
			}
		}

		public override void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, degubDrawRadius);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _connectiviyRadius);
		}

		public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
		{
			if (_connections.Count == 0)
			{
				Debug.LogError("Insufficient waypoint count.");
				return null;
			}
			else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
			{
				return previousWaypoint;
			}
			else
			{
				ConnectedWaypoint nextWaypoint;

				int nextIndex;

				do
				{
					nextIndex = UnityEngine.Random.Range(0, _connections.Count);
					nextWaypoint = _connections[nextIndex];
				} while (nextWaypoint == previousWaypoint);

				return nextWaypoint;
			}
		}
	}
}