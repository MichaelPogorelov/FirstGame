using System.Collections.Generic;
using System.Threading.Tasks;
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

		public async Task Warmup()
		{
			await _assets.Load<GameObject>(AssetAdress.LootPath);
			await _assets.Load<GameObject>(AssetAdress.Spawner);
		}

		public GameObject CreateKnight(Vector3 at)
		{
			_knightGameObject = InstantiateRegister(AssetAdress.KnightPath, at);
			return _knightGameObject;
		}

		public GameObject CreateHud()
		{
			GameObject hud = InstantiateRegister(AssetAdress.HudPath);
			hud.GetComponentInChildren<LootCounter>().Constructor(_progressService.Progress.WorldData);
			foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
			{
				openWindowButton.Constructor(_windowService);
			}
			
			return hud;
		}

		public async Task<GameObject> CreateEnemy(EnemyType type, Transform parent)
		{
			EnemyStaticData enemyData = _staticData.ForEnemy(type);

			GameObject prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);
			
			GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
			
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

		public async Task<LootPiece> CreateLoot(Vector3 position)
		{
			var prefab = await _assets.Load<GameObject>(AssetAdress.LootPath);
			
			LootPiece lootPiece = InstantiateRegister(prefab, position).GetComponent<LootPiece>();
			lootPiece.Constructor(_progressService.Progress.WorldData);

			return lootPiece;
		}

		public async Task CreateSpawner(Vector3 at, string id, EnemyType type)
		{
			var prefab = await _assets.Load<GameObject>(AssetAdress.Spawner);
			
			SpawnPoint spawner = InstantiateRegister(prefab, at).GetComponent<SpawnPoint>();
			spawner.Constructor(this);
			spawner.EnemyType = type;
			spawner.Id = id;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
			_assets.Cleanup();
		}
		
		private GameObject InstantiateRegister(GameObject prefab, Vector3 at)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab, at, Quaternion.identity);
			RegisterProgress(gameObject);
			return gameObject;
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