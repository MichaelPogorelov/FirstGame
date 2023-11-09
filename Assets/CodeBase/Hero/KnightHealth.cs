using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(KnightAnimator))]
	public class KnightHealth : MonoBehaviour, ISaveProgress
	{
		public KnightAnimator Animator;
		public event Action HealthChange;
		
		private PlayerHealthData _playerHealth;

		public float CurrentHP { get => _playerHealth.CurrentHP;
			set
			{
				if (_playerHealth.CurrentHP != value)
				{
					_playerHealth.CurrentHP = value;
					HealthChange?.Invoke();	
				}
			}
		}
		public float CurrentMaxHP { get => _playerHealth.MaxHP; set => _playerHealth.MaxHP = value; }
		
		public void LoadProgress(PlayerProgress progress)
		{
			_playerHealth = progress.PlayerHealth;
			HealthChange?.Invoke();
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			progress.PlayerHealth.CurrentHP = CurrentHP;
			progress.PlayerHealth.MaxHP = CurrentMaxHP;
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