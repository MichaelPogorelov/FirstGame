using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	public class RotateToPlayer : Follow
	{
		public float Speed;

		private Transform _playerTransform;
		private IGameFactory _gameFactory;
		private Vector3 _positionToLook;

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
			if (_playerTransform != null)
			{
				Rotate();
			}
		}

		private void Rotate()
		{
			UpdatePositionToLook();
			transform.rotation = SmoothRotation(transform.rotation, _positionToLook);
		}

		private void SetPlayerTransform()
		{
			_playerTransform = _gameFactory.KnightGameObject.transform;
		}

		private void UpdatePositionToLook()
		{
			Vector3 positionDiff = _playerTransform.position - transform.position;
			_positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
		}

		private Quaternion SmoothRotation(Quaternion rotation, Vector3 positionToLook)
		{
			return Quaternion.Lerp(rotation, Quaternion.LookRotation(positionToLook), Speed * Time.deltaTime);
		}
	}
}