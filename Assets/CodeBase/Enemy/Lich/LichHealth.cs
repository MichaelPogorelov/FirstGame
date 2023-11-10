using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(LichAnimator))]
	public class LichHealth : MonoBehaviour, IHealth
	{
		public LichAnimator LichAnimator;
		[SerializeField] private float _currentHP;
		[SerializeField] private float _maxHP;

		public event Action HealthChanged;

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
			LichAnimator.PlayHit();
			HealthChanged?.Invoke();
		}
	}
}