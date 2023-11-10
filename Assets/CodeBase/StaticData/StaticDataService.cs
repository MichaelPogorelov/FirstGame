using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private Dictionary<EnemyType,EnemyStaticData> _enemies;

		public void LoadEnemy()
		{
			_enemies = Resources.LoadAll<EnemyStaticData>("StaticData").ToDictionary(x => x.EnemyType, x => x);
			Debug.Log(_enemies.Count);
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
	}
}