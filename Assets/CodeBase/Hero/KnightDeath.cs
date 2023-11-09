using System;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(KnightHealth))]
	public class KnightDeath : MonoBehaviour
	{
		public KnightHealth KnightHealth;
		public KnightMove KnightMove;
		public KnightAnimator KnightAnimator;
		
		public GameObject DeathFX;
		private bool _isDeath;

		private void Start()
		{
			KnightHealth.HealthChange += HealthChange;
		}

		private void OnDestroy()
		{
			KnightHealth.HealthChange -= HealthChange;
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
			KnightAnimator.PlayDeath();

			Instantiate(DeathFX, transform.position, Quaternion.identity);
		}
	}
}