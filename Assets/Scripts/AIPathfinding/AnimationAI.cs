using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AIPathfinding
{
	public class AnimationAI : MonoBehaviour
	{
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private Weapon _rifle;
		[SerializeField] private Weapon _handgun;

		[SerializeField] private Animator _animator;

		[SerializeField] private NPCBase _npcBase;
		private void Awake()
		{
			_handgun.gameObject.SetActive(false);
			_rifle.gameObject.SetActive(false);
		}

		private void Start()
		{
			_rifle.Fired += OnFired;
			_handgun.Fired += OnFired;

			_animator.SetBool("isHandgun", _npcBase.UsingHandgun);

			if (_npcBase.UsingHandgun)
			{
				_handgun.gameObject.SetActive(true);
			}
			else
			{
				_rifle.gameObject.SetActive(true);
			}
		}

		private void OnFired()
		{
			_animator.Play("Fire", 1);
		}

		private void Update()
		{
			float x = Vector3.Dot(transform.right, _navMeshAgent.velocity);
			float y = Vector3.Dot(transform.forward, _navMeshAgent.velocity);

			_animator.SetFloat("Vertical", y);
			_animator.SetFloat("Horizontal", x);
		}
	}
}