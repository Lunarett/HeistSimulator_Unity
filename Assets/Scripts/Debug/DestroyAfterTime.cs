using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField] private float _seconds;

	private void Start()
	{
		StartCoroutine(DestroyAfterSeconds());
	}

	IEnumerator DestroyAfterSeconds()
	{
		yield return new WaitForSeconds(_seconds);

		Destroy(gameObject);
	}
}
