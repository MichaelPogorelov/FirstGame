using UnityEngine;

namespace CodeBase.CameraLogic
{
	public class CameraFollow : MonoBehaviour
	{
		public float RotationAngelX;
		public float Distance;
		public float OffsetY;
		[SerializeField] private Transform _following;

		private void LateUpdate()
		{
			if (_following == null)
			{
				return;
			}

			CameraMove();
		}

		public void Follow(GameObject following)
		{
			_following = following.transform;
		}

		private void CameraMove()
		{
			Quaternion rotation = Quaternion.Euler(RotationAngelX, 0, 0);
			Vector3 position = rotation * new Vector3(0, 0, -Distance) + FollowingPointPosition();
			transform.rotation = rotation;
			transform.position = position;
		}

		private Vector3 FollowingPointPosition()
		{
			Vector3 followingPosition = _following.position;
			followingPosition.y += OffsetY;
			return followingPosition;
		}
	}
}