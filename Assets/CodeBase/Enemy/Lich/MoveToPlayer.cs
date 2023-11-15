using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class MoveToPlayer : Follow
	{
		public NavMeshAgent Agent;

		private const float MinimalDistance = 1f;
		private Transform _playerTransform;
		private IGameFactory _gameFactory;

		public void Constructor(Transform knightTransform)
		{
			_playerTransform = knightTransform;
		}
		
		private void Update()
		{
			if (_playerTransform != null && PlayerNotReached())
			{
				Agent.destination = _playerTransform.position;
			}
		}

		private bool PlayerNotReached()
		{
			return Vector3.Distance(Agent.transform.position, _playerTransform.position) >= MinimalDistance;
		}
	}
}