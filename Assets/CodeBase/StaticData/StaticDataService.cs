using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using CodeBase.UI.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private const string StaticDataEnemiesPath = "StaticData/Enemies";
		private const string StaticDataLevelsPath = "StaticData/Levels";
		private const string StaticDataWindowsPath = "StaticData/Windows/WindowData";
		
		private Dictionary<EnemyType, EnemyStaticData> _enemies;
		private Dictionary<string, LevelStaticData> _levels;
		private Dictionary<WindowType, WindowConfig> _windows;

		public void LoadStaticData()
		{
			_enemies = Resources.LoadAll<EnemyStaticData>(StaticDataEnemiesPath).ToDictionary(x => x.EnemyType, x => x);
			_levels = Resources.LoadAll<LevelStaticData>(StaticDataLevelsPath).ToDictionary(x => x.LevelKey, x => x);
			_windows = Resources.Load<WindowStaticData>(StaticDataWindowsPath).Windows.ToDictionary(x => x.Type, x => x);
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

		public WindowConfig ForWindow(WindowType type)
		{
			if (_windows.TryGetValue(type, out WindowConfig config))
			{
				return config;
			}
			else
			{
				return null;
			}
		}
	}
}