using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : Interactable
{
	[FMODUnity.EventRef]
	[SerializeField] private string _money;

	public string ItemType;
	public float ItemValue;
	public float ItemWeight;
	public bool _isMoney;

	public override void Interact(PlayerInteractController interactor)
	{
		if (interactor.PlayerCurrentLoad < interactor.PlayerMaxLoadCapacity)
		{
			base.Interact(interactor);
			interactor.WalletAmount += ItemValue;
			interactor.PlayerCurrentLoad += ItemWeight;

			FMODUnity.RuntimeManager.PlayOneShot(_money, gameObject.transform.position);
			Destroy(gameObject);
		}
	}
}
