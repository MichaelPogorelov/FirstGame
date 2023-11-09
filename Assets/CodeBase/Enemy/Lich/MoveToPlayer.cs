using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
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

		private void Start()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();
			if (_gameFactory.KnightGameObject != null)
			{
				SetPlayerTransform();
			}
			else
			{
				_gameFactory.KnightCreated += SetPlayerTransform;
			}
		}

		private void Update()
		{
			if (_playerTransform != null && PlayerNotReached())
			{
				Agent.destination = _playerTransform.position;
			}
		}

		private void SetPlayerTransform()
		{
			_playerTransform = _gameFactory.KnightGameObject.transform;
		}

		private bool PlayerNotReached()
		{
			return Vector3.Distance(Agent.transform.position, _playerTransform.position) >= MinimalDistance;
		}
	}
}