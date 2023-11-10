using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(KnightHealth))]
	public class KnightDeath : MonoBehaviour
	{
		public KnightHealth KnightHealth;
		public KnightMove KnightMove;
		public KnightAttack KnightAttack;
		public KnightAnimator KnightAnimator;
		
		public GameObject DeathFX;
		private bool _isDeath;

		private void Start()
		{
			KnightHealth.HealthChanged += HealthChange;
		}

		private void OnDestroy()
		{
			KnightHealth.HealthChanged -= HealthChange;
		}

		private void HealthChange()
		{
			if (KnightHealth.CurrentHP <= 0 && !_isDeath)
			{
				Die();
			}
		}

		private void Die()
		{
			_isDeath = true;
			KnightMove.enabled = false;
			KnightAttack.enabled = false;
			KnightAnimator.PlayDeath();

			Instantiate(DeathFX, transform.position, Quaternion.identity);
		}
	}
}