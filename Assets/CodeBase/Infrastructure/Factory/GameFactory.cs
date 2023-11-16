using System.Collections.Generic;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Loot;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		public List<ISaveProgressReader> ProgressReaders { get; } = new List<ISaveProgressReader>();
		public List<ISaveProgress> ProgressWriters { get; } = new List<ISaveProgress>();

		private readonly IAssetProvider _assets;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomService;
		private GameObject _knightGameObject;

		public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService randomService)
		{
			_assets = assets;
			_staticData = staticData;
			_randomService = randomService;
		}

		public GameObject CreateKnight(GameObject at)
		{
			_knightGameObject = InstantiateRegister(AssetPath.KnightPath, at.transform.position);
			return _knightGameObject;
		}

		public GameObject CreateHud()
		{
			return InstantiateRegister(AssetPath.HudPath);
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

		public GameObject CreateLoot()
		{
			return InstantiateRegister(AssetPath.LootPath);
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		public void Register(ISaveProgressReader progressReader)
		{
			if (progressReader is ISaveProgress progressWriters)
			{
				ProgressWriters.Add(progressWriters);
			}
			ProgressReaders.Add(progressReader);
		}

		private GameObject InstantiateRegister(string prefabPath, Vector3 at)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath, at);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegister(string prefabPath)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private void RegisterProgressWatchers(GameObject gameObject)
		{
			foreach (ISaveProgressReader progressReader in gameObject.GetComponentsInChildren<ISaveProgressReader>())
			{
				Register(progressReader);
			}
		}
	}
}