using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	public class AggroZoneMove : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public MoveToPlayer MoveToPlayer;

		private void Start()
		{
			TriggerObserver.TriggerEnter += EnterAggroZone;
			TriggerObserver.TriggerExit += ExitAggroZone;

			MoveToPlayer.enabled = false;
		}

		private void EnterAggroZone(Collider obj)
		{
			MoveToPlayer.enabled = true;
		}

		private void ExitAggroZone(Collider obj)
		{
			MoveToPlayer.enabled = false;
		}
	}
}