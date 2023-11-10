using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
	public class EnemySpawner : MonoBehaviour, ISaveProgress
	{
		public EnemyType EnemyType;

		private string _id;
		public bool _isDeath;

		private void Awake()
		{
			_id = GetComponent<UniqueId>().Id;
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
		}
	}
}