using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	public class RotateToPlayer : Follow
	{
		public float Speed;

		private Transform _playerTransform;
		private Vector3 _positionToLook;

		public void Constructor(Transform knightTransform)
		{
			_playerTransform = knightTransform;
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