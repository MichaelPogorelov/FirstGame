using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(KnightAnimator), typeof(CharacterController))]
	public class KnightAttack : MonoBehaviour, ISaveProgressReader
	{
		public KnightAnimator KnightAnimator;
		public CharacterController CharacterController;

		private IInputService _input;
		private static int _layerMask;
		private Collider[] _hits = new Collider[3];
		private PlayerAttackData _playerAttack;

		private void Awake()
		{
			_input = AllServices.Container.Single<IInputService>();
			_layerMask = 1 << LayerMask.NameToLayer("Hittable");
		}

		private void Update()
		{
			if (_input.IsAttackButtonUp() && !KnightAnimator.IsAttacking)
			{
				KnightAnimator.PlayAttack();
			}
		}

		public void KnightAnimatorOnAttack() // animator event
		{
			for (int i = 0; i < Hit(); i++)
			{
				_hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_playerAttack.Damage);
			}
		}

		private int Hit()
		{
			return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _playerAttack.RadiusAttack, _hits, _layerMask);
		}

		private Vector3 StartPoint()
		{
			return new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
		}

		public void LoadProgress(PlayerProgress progress)
		{
			_playerAttack = progress.PlayerAttack;
		}
	}
}