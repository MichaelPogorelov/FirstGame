using UnityEngine;

namespace CodeBase.Loot
{
	public class EndlessRotation : MonoBehaviour
	{
		public float Speed = 0.1f;

		private void Update()
		{
			transform.rotation *= Quaternion.Euler(0, Speed * Time.deltaTime, 0);
		}
	}
}