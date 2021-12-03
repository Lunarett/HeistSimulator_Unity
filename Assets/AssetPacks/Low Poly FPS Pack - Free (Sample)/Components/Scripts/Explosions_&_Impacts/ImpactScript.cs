using UnityEngine;


public class ImpactScript : MonoBehaviour
{

	[Header("Impact Despawn Timer")]
	[SerializeField] private float _despawnTimer = 10.0f;
	[FMODUnity.EventRef]
	[SerializeField] private string _impact;

	private void Start()
	{
		FMODUnity.RuntimeManager.PlayOneShotAttached(_impact, gameObject);

		Destroy(gameObject, _despawnTimer);
	}
}
