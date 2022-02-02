using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float _lifetime = 0.7f;
	[SerializeField] private LayerMask _damageLayerMask;
	[SerializeField] private float _damage;

	private void Start()
	{
		StartCoroutine(Delay(_lifetime));
	}

	IEnumerator Delay(float lifetime)
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Ground"))
		{
			Destroy(gameObject);
		}
	}
}
