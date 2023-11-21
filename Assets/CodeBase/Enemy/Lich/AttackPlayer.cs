using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class AttackPlayer : MonoBehaviour
	{
		public EnemyAnimator enemyAnimator;
		public float AttackCooldown = 1f;
		public float HitRadius = 0.5f;
		public float ForwardDistanceCoef = 0.5f;
		public float Damage = 10f;

		private IGameFactory _gameFactory;
		private Transform _playerTransform;
		private float _attackCooldown;
		private bool _isAttacking;
		private int _layerMask;
		private Collider[] _hits = new Collider[1];
		private bool _attackIsActive;

		public void Constructor(Transform playerTransform)
		{
			_playerTransform = playerTransform;
		}

		private void Awake()
		{
			_layerMask = 1 << LayerMask.NameToLayer("Player");
		}

		private void Update()
		{
			UpdateCooldown();
			
			if (CanAttack())
			{
				StartAttack();
			}
		}

		private void AnimationEventLichAttack() // animator event
		{
			if (Hit(out Collider hit))
			{
				hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
			}
		}

		private bool Hit(out Collider hit)
		{
			int hitsCount = Physics.OverlapSphereNonAlloc(HitPointPosition(), HitRadius, _hits, _layerMask);

			hit = _hits.FirstOrDefault();
			return hitsCount > 0;
		}

		private void AnimationEventLichAttack2() // animator event
		{
			
		}

		private void AnimationEventLichFinishAttack() // animator event
		{
			_attackCooldown = AttackCooldown;
			_isAttacking = false;
		}

		private void AnimationEventLichFinishAttack2() // animator event
		{
			
		}

		public void EnableAttack()
		{
			_attackIsActive = true;
		}

		public void DisableAttack()
		{
			_attackIsActive = false;
		}

		private void StartAttack()
		{
			transform.LookAt(_playerTransform);
			enemyAnimator.PlayAttack1();
			_isAttacking = true;
		}

		private void UpdateCooldown()
		{
			if (_attackCooldown > 0)
			{
				_attackCooldown -= Time.deltaTime;
			}
		}

		private bool CanAttack()
		{
			return _attackIsActive && _attackCooldown <= 0 && !_isAttacking;
		}

		private Vector3 HitPointPosition()
		{
			return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * ForwardDistanceCoef;
		}
	}
}