using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(LichHealth), typeof(LichAnimator))]
	public class LichDeath : MonoBehaviour
	{
		public LichHealth LichHealth;
		public LichAnimator LichAnimator;

		public GameObject DeathFX;
		public event Action DeathHappend;

		private void Start()
		{
			LichHealth.HealthChanged += HealthChange;
		}

		private void OnDestroy()
		{
			LichHealth.HealthChanged -= HealthChange;
		}

		private void HealthChange()
		{
			if (LichHealth.CurrentHP < 0)
			{
				Die();
			}
		}

		private void Die()
		{
			LichHealth.HealthChanged -= HealthChange;
			
			LichAnimator.PlayDeath();
			SpawnFX();
			StartCoroutine(DestroyLich());
			
			DeathHappend?.Invoke();
		}

		private void SpawnFX()
		{
			Instantiate(DeathFX, transform.position, Quaternion.identity);
		}

		private IEnumerator DestroyLich()
		{
			yield return new WaitForSeconds(3);
			Destroy(gameObject);
		}
	}
}