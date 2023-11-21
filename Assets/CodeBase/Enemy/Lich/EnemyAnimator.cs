using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
	{
		public event Action<AnimatorState> StateEntered; 
		public event Action<AnimatorState> StateExited;
		public AnimatorState State { get; private set; }

		private Animator _animator;
		private static readonly int _attack1 = Animator.StringToHash("Attack_1");
		private static readonly int _attack2 = Animator.StringToHash("Attack_2");
		private static readonly int _speed = Animator.StringToHash("Speed");
		private static readonly int _isMoving = Animator.StringToHash("IsMoving");
		private static readonly int _hit = Animator.StringToHash("Hit");
		private static readonly int _die = Animator.StringToHash("Die");
		private static readonly int _win = Animator.StringToHash("Win");

		private readonly int _idleStateHash = Animator.StringToHash("idle");
		private readonly int _attack1StateHash = Animator.StringToHash("attack01");
		private readonly int _attack2StateHash = Animator.StringToHash("attack02");
		private readonly int _walkingStateHash = Animator.StringToHash("Move");
		private readonly int _deathStateHash = Animator.StringToHash("die");
		private readonly int _winStateHash = Animator.StringToHash("victory");
		private readonly int _hitStateHash = Animator.StringToHash("GetHit");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public void PlayHit()
		{
			_animator.SetTrigger(_hit);
		}

		public void PlayDeath()
		{
			_animator.SetTrigger(_die);
		}

		public void PlayWin()
		{
			_animator.SetTrigger(_win);
		}

		public void PlayAttack1()
		{
			_animator.SetTrigger(_attack1);
		}

		public void PlayAttack2()
		{
			_animator.SetTrigger(_attack2);
		}

		public void PlayMove(float speed)
		{
			_animator.SetBool(_isMoving, true);
			_animator.SetFloat(_speed, speed);
		}

		public void StopPlayMove()
		{
			_animator.SetBool(_isMoving, false);
		}

		public void EnteredState(int stateHash)
		{
			State = StateFor(stateHash);
			StateEntered?.Invoke(State);
		}

		public void ExitedState(int stateHash)
		{
			StateExited?.Invoke(StateFor(stateHash));
		}

		private AnimatorState StateFor(int stateHash)
		{
			AnimatorState state;
			if (stateHash == _idleStateHash)
			{
				state = AnimatorState.Idle;
			}
			else if (stateHash == _attack1StateHash)
			{
				state = AnimatorState.Attack1;
			}
			else if (stateHash == _attack2StateHash)
			{
				state = AnimatorState.Attack2;
			}
			else if (stateHash == _walkingStateHash)
			{
				state = AnimatorState.Walking;
			}
			else if (stateHash == _deathStateHash)
			{
				state = AnimatorState.Died;
			}
			else if (stateHash == _hitStateHash)
			{
				state = AnimatorState.Hit;
			}
			else if (stateHash == _winStateHash)
			{
				state = AnimatorState.Win;
			}
			else
			{
				state = AnimatorState.Unknown;
			}

			return state;
		}
	}
}