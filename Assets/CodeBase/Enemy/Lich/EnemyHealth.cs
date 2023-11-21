using System;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class EnemyHealth : MonoBehaviour, IHealth
	{
		public EnemyAnimator enemyAnimator;
		public ActorUI ActorUI;
		[SerializeField] private float _currentHP;
		[SerializeField] private float _maxHP;

		public event Action HealthChanged;

		private void Awake()
		{
			ActorUI.Constructor(this);
		}

		public float CurrentHP
		{
			get => _currentHP;
			set => _currentHP = value;
		}

		public float MaxHP
		{
			get => _maxHP;
			set => _maxHP = value;
		}

		public void TakeDamage(float damage)
		{
			CurrentHP -= damage;
			enemyAnimator.PlayHit();
			HealthChanged?.Invoke();
		}
	}
}