using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(KnightAnimator))]
	public class KnightHealth : MonoBehaviour, ISaveProgress, IHealth
	{
		public KnightAnimator Animator;
		public event Action HealthChanged;

		private PlayerHealthData _playerHealth;

		public float CurrentHP { get => _playerHealth.CurrentHP;
			set
			{
				if (_playerHealth.CurrentHP != value)
				{
					_playerHealth.CurrentHP = value;
					HealthChanged?.Invoke();	
				}
			}
		}
		public float MaxHP { get => _playerHealth.MaxHP; set => _playerHealth.MaxHP = value; }
		
		
		public void LoadProgress(PlayerProgress progress)
		{
			_playerHealth = progress.PlayerHealth;
			HealthChanged?.Invoke();
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			progress.PlayerHealth.CurrentHP = CurrentHP;
			progress.PlayerHealth.MaxHP = MaxHP;
		}

		public void TakeDamage(float damage)
		{
			if (CurrentHP <= 0)
			{
				return;
			}

			CurrentHP -= damage;
			Animator.PlayHit();
		}
	}
}