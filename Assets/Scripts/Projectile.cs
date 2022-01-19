using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private LayerMask _damageLayerMask;
	[SerializeField] private float _damage;

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == _damageLayerMask)
		{

		}
		
		Destroy(gameObject);
	}
}
