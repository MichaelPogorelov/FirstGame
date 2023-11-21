using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(EnemyAnimator))]
	public class AnimateLichMove : MonoBehaviour
	{
		private const float MinimalVelocity = 0.1f;

		public NavMeshAgent NavMeshAgent;
		public EnemyAnimator enemyAnimator;

		private void Update()
		{
			if (ShouldMove())
			{
				enemyAnimator.PlayMove(NavMeshAgent.velocity.magnitude);
			}
			else
			{
				enemyAnimator.StopPlayMove();
			}
		}

		private bool ShouldMove()
		{
			return NavMeshAgent.velocity.magnitude > MinimalVelocity && NavMeshAgent.remainingDistance > NavMeshAgent.radius;
		}
	}
}