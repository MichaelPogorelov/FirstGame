using System.Collections.Generic;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Loot;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		public List<ILoadProgress> ProgressReaders { get; } = new List<ILoadProgress>();
		public List<ISaveProgress> ProgressWriters { get; } = new List<ISaveProgress>();

		private readonly IAssetProvider _assets;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomService;
		private readonly IPersistentProgressService _progressService;
		private readonly IWindowService _windowService;
		private GameObject _knightGameObject;

		public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
		{
			_assets = assets;
			_staticData = staticData;
			_randomService = randomService;
			_progressService = progressService;
			_windowService = windowService;
		}

		public GameObject CreateKnight(GameObject at)
		{
			_knightGameObject = InstantiateRegister(AssetPath.KnightPath, at.transform.position);
			return _knightGameObject;
		}

		public GameObject CreateHud()
		{
			GameObject hud = InstantiateRegister(AssetPath.HudPath);
			hud.GetComponentInChildren<LootCounter>().Constructor(_progressService.Progress.WorldData);
			foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
			{
				openWindowButton.Constructor(_windowService);
			}
			
			return hud;
		}

		public GameObject CreateEnemy(EnemyType type, Transform parent)
		{
			EnemyStaticData enemyData = _staticData.ForEnemy(type);
			GameObject enemy = Object.Instantiate(enemyData.Prefab, parent.position, Quaternion.identity, parent);
			
			IHealth health = enemy.GetComponent<IHealth>();
			health.CurrentHP = enemyData.HP;
			health.MaxHP = enemyData.HP;
			
			enemy.GetComponent<ActorUI>().Constructor(health);
			enemy.GetComponent<MoveToPlayer>()?.Constructor(_knightGameObject.transform);
			enemy.GetComponent<RotateToPlayer>()?.Constructor(_knightGameObject.transform);
			enemy.GetComponent<NavMeshAgent>().speed = enemyData.MoveSpeed;

			AttackPlayer attack = enemy.GetComponent<AttackPlayer>();
			attack.Constructor(_knightGameObject.transform);
			attack.Damage = enemyData.Damage;
			attack.HitRadius = enemyData.HitRadius;
			attack.AttackCooldown = enemyData.AttackCooldown;
			attack.ForwardDistanceCoef = enemyData.ForwardDistanceCoef;

			LootSpawner loot = enemy.GetComponentInChildren<LootSpawner>();
			loot.Constructor(this, _randomService);
			loot.SetLoot(enemyData.MinLoot, enemyData.MaxLoot);
			
			return enemy;
		}

		public LootPiece CreateLoot(Vector3 position)
		{
			LootPiece lootPiece = InstantiateRegister(AssetPath.LootPath, position).GetComponent<LootPiece>();
			lootPiece.Constructor(_progressService.Progress.WorldData);

			return lootPiece;
		}

		public void CreateSpawner(Vector3 at, string id, EnemyType type)
		{
			SpawnPoint spawner = InstantiateRegister(AssetPath.Spawner, at).GetComponent<SpawnPoint>();
			spawner.Constructor(this);
			spawner.EnemyType = type;
			spawner.Id = id;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		private GameObject InstantiateRegister(string prefabPath, Vector3 at)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath, at);
			RegisterProgress(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegister(string prefabPath)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath);
			RegisterProgress(gameObject);
			return gameObject;
		}

		private void RegisterProgress(GameObject gameObject)
		{
			foreach (ISaveLoadProgress progress in gameObject.GetComponentsInChildren<ISaveLoadProgress>())
			{
				if (progress is ILoadProgress loadProgress)
				{
					RegisterLoadProgress(loadProgress);
				}
				if (progress is ISaveProgress saveProgress)
				{
					RegisterSaveProgress(saveProgress);
				}
			}
		}

		private void RegisterLoadProgress(ILoadProgress progress)
		{
			ProgressReaders.Add(progress);
		}

		private void RegisterSaveProgress(ISaveProgress progress)
		{
			ProgressWriters.Add(progress);
		}
	}
}