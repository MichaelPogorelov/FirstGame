using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(LichAnimator))]
	public class AnimateLichMove : MonoBehaviour
	{
		private const float MinimalVelocity = 0.1f;

		public NavMeshAgent NavMeshAgent;
		public LichAnimator LichAnimator;

		private void Update()
		{
			if (ShouldMove())
			{
				LichAnimator.PlayMove(NavMeshAgent.velocity.magnitude);
			}
			else
			{
				LichAnimator.StopPlayMove();
			}
		}

		private bool ShouldMove()
		{
			return NavMeshAgent.velocity.magnitude > MinimalVelocity && NavMeshAgent.remainingDistance > NavMeshAgent.radius;
		}
	}
}