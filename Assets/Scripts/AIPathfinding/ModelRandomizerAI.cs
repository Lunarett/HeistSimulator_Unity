
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AIPathfinding
{
	public class ModelRandomizerAI : MonoBehaviour
	{
		[SerializeField] private GameObject[] _models;
		[SerializeField] private GameObject[] _leftItems;
		[SerializeField] private GameObject[] _rightItems;
		[SerializeField] private GameObject _duffelBag;

		[SerializeField] [Range(0, 1)] private float _leftChance;
		[SerializeField] [Range(0, 1)] private float _rightChance;
		[SerializeField] [Range(0, 1)] private float _bagChance;

		private void Start()
		{
			_models[0].SetActive(false);

			SelectRandom(_models).SetActive(true);

			if(Random.value > _leftChance)
			{
				SelectRandom(_leftItems).SetActive(true);
			}
			if (Random.value > _rightChance)
			{
				SelectRandom(_rightItems).SetActive(true);
			}
			if (Random.value > _bagChance)
			{
				_duffelBag.SetActive(true);
			}
		}

		private T SelectRandom<T>(T[] objects)
		{
			return objects[Random.Range(0, objects.Length)];
		}
	}
}