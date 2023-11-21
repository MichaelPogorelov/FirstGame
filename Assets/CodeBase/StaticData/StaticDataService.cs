using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private Dictionary<EnemyType, EnemyStaticData> _enemies;
		private Dictionary<string, LevelStaticData> _levels;

		public void LoadEnemy()
		{
			_enemies = Resources.LoadAll<EnemyStaticData>("StaticData/Enemies").ToDictionary(x => x.EnemyType, x => x);
			_levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels").ToDictionary(x => x.LevelKey, x => x);
		}

		public EnemyStaticData ForEnemy(EnemyType type)
		{
			if (_enemies.TryGetValue(type, out EnemyStaticData staticData))
			{
				return staticData;
			}
			else
			{
				return null;
			}
		}

		public LevelStaticData ForLevel(string sceneName)
		{
			if (_levels.TryGetValue(sceneName, out LevelStaticData staticData))
			{
				return staticData;
			}
			else
			{
				return null;
			}
		}
	}
}