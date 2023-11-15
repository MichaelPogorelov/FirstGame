using CodeBase.Data;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
	public class EnemySpawner : MonoBehaviour, ISaveProgress
	{
		public EnemyType EnemyType;

		private string _id;
		public bool _isDeath;
		private IGameFactory _gameFactory;
		private LichDeath _lichDeath;

		private void Awake()
		{
			_id = GetComponent<UniqueId>().Id;
			_gameFactory = AllServices.Container.Single<IGameFactory>();
		}

		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.EnemyDeath.ClearedSpawners.Contains(_id))
			{
				_isDeath = true;
			}
			else
			{
				Spawn();
			}
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if (_isDeath)
			{
				progress.EnemyDeath.ClearedSpawners.Add(_id);
			}
		}

		private void Spawn()
		{
			GameObject enemy = _gameFactory.CreateEnemy(EnemyType, transform);
			_lichDeath = enemy.GetComponent<LichDeath>();
			_lichDeath.DeathHappend += Slay;
		}

		private void Slay()
		{
			_isDeath = true;
			if (_lichDeath != null)
			{
				_lichDeath.DeathHappend -= Slay;
			}
		}
	}
}