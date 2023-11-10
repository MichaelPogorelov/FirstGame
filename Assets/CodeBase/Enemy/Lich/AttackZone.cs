using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(AttackPlayer))]
	public class AttackZone : MonoBehaviour
	{
		public AttackPlayer AttackPlayer;
		public TriggerObserver TriggerObserver;

		private void Start()
		{
			TriggerObserver.TriggerEnter += EnterAttackZone;
			TriggerObserver.TriggerExit += ExitAttackZone;
			
			AttackPlayer.DisableAttack();
		}

		private void EnterAttackZone(Collider obj)
		{
			AttackPlayer.EnableAttack();
		}

		private void ExitAttackZone(Collider obj)
		{
			AttackPlayer.DisableAttack();
		}
	}
}