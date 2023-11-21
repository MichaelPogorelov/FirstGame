using CodeBase.Data;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
	public class EnemySpawner : MonoBehaviour, ISaveProgress, ILoadProgress
	{
		public string Id;
		public EnemyType EnemyType;

		private bool _isDeath;
		private IGameFactory _gameFactory;
		private EnemyDeath _enemyDeath;

		private void Awake()
		{
			Id = GetComponent<UniqueId>().Id;
			_gameFactory = AllServices.Container.Single<IGameFactory>();
		}

		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.EnemyDeath.ClearedSpawners.Contains(Id))
			{
				_isDeath = true;
			}
			else
			{
				Spawn();
			}
		}

		public void SaveProgress(PlayerProgress progress)
		{
			if (_isDeath)
			{
				progress.EnemyDeath.ClearedSpawners.Add(Id);
			}
		}

		private void Spawn()
		{
			GameObject enemy = _gameFactory.CreateEnemy(EnemyType, transform);
			_enemyDeath = enemy.GetComponent<EnemyDeath>();
			_enemyDeath.DeathHappend += Slay;
		}

		private void Slay()
		{
			_isDeath = true;
			if (_enemyDeath != null)
			{
				_enemyDeath.DeathHappend -= Slay;
			}
		}
	}
}