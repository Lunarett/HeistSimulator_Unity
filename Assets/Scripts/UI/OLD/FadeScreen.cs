using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
	[SerializeField] private bool _fadeIn;
	[SerializeField] private float _fadeDuration = 2;

	public Animator FadeAnimator { get; set; } 

	private void Awake()
	{
		gameObject.SetActive(true);

		FadeAnimator = GetComponent<Animator>();

		if (FadeAnimator != null)
		{
			FadeAnimator.SetBool("In", _fadeIn);
			StartCoroutine(DestroyAfterSeconds(_fadeDuration));
		}
	}

	IEnumerator DestroyAfterSeconds(float seconds)
	{ 
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
