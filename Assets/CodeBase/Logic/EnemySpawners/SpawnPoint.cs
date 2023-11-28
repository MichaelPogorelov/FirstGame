using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
	public class SpawnPoint : MonoBehaviour, ISaveProgress, ILoadProgress
	{
		public string Id { get; set; }
		public EnemyType EnemyType;

		private bool _isDeath;
		private IGameFactory _gameFactory;
		private EnemyDeath _enemyDeath;

		public void Constructor(IGameFactory gameFactory)
		{
			_gameFactory = gameFactory;
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

		private async void Spawn()
		{
			GameObject enemy = await _gameFactory.CreateEnemy(EnemyType, transform);
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