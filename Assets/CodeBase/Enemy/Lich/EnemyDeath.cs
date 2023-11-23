using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
	public class EnemyDeath : MonoBehaviour
	{
		public EnemyHealth enemyHealth;
		public EnemyAnimator enemyAnimator;

		public GameObject DeathFX;
		public event Action DeathHappend;

		private void Start()
		{
			enemyHealth.HealthChanged += HealthChange;
		}

		private void OnDestroy()
		{
			enemyHealth.HealthChanged -= HealthChange;
		}

		private void HealthChange()
		{
			if (enemyHealth.CurrentHP < 0)
			{
				Die();
			}
		}

		private void Die()
		{
			enemyHealth.HealthChanged -= HealthChange;
			DeathHappend?.Invoke();
			enemyHealth.ActorUI.HpBar.gameObject.SetActive(false);
			enemyAnimator.PlayDeath();
			SpawnFX();
			StartCoroutine(DestroyLich());
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