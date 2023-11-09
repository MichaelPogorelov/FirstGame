using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy.Lich
{
	[RequireComponent(typeof(LichAnimator))]
	public class AttackPlayer : MonoBehaviour
	{
		public LichAnimator LichAnimator;
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

		private void Awake()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();
			_gameFactory.KnightCreated += OnPlayerCreated;
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

		private void AnimationEventLichAttack()
		{
			if (Hit(out Collider hit))
			{
				hit.transform.GetComponent<KnightHealth>().TakeDamage(Damage);
			}
		}

		private bool Hit(out Collider hit)
		{
			int hitsCount = Physics.OverlapSphereNonAlloc(HitPointPosition(), HitRadius, _hits, _layerMask);

			hit = _hits.FirstOrDefault();
			return hitsCount > 0;
		}

		private void AnimationEventLichAttack2()
		{
			
		}

		private void AnimationEventLichFinishAttack()
		{
			_attackCooldown = AttackCooldown;
			_isAttacking = false;
		}

		private void AnimationEventLichFinishAttack2()
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
			LichAnimator.PlayAttack1();
			_isAttacking = true;
		}

		private void OnPlayerCreated()
		{
			_playerTransform = _gameFactory.KnightGameObject.transform;
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