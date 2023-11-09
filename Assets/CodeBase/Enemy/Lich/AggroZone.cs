using System.Collections;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	public class AggroZone : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public Follow MoveToPlayer;
		private float _cooldown = 3f;
		private Coroutine _aggroCoroutine;
		private bool _hasAggroTarget;

		private void Start()
		{
			TriggerObserver.TriggerEnter += EnterAggroZone;
			TriggerObserver.TriggerExit += ExitAggroZone;

			MoveToPlayer.enabled = false;
		}

		private void EnterAggroZone(Collider obj)
		{
			if (!_hasAggroTarget)
			{
				_hasAggroTarget = true;
				StopAndResetCoroutine();
				MoveToPlayer.enabled = true;
			}
		}

		private void ExitAggroZone(Collider obj)
		{
			if (_hasAggroTarget)
			{
				_hasAggroTarget = false;
				_aggroCoroutine = StartCoroutine(StopMoveAfterCooldown());
			}
		}

		private IEnumerator StopMoveAfterCooldown()
		{
			yield return new WaitForSeconds(_cooldown);
			MoveToPlayer.enabled = false;
		}

		private void StopAndResetCoroutine()
		{
			if (_aggroCoroutine != null)
			{
				StopCoroutine(_aggroCoroutine);
				_aggroCoroutine = null;
			}
		}
	}
}