using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Loot;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string InitialPointTag = "InitialPoint";
		private const string EnemySpawnerTag = "EnemySpawner";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtaine;
		private readonly IGameFactory _factory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticData;

		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtaine, 
			IGameFactory factory, IPersistentProgressService progressService, IStaticDataService staticData)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtaine = curtaine;
			_factory = factory;
			_progressService = progressService;
			_staticData = staticData;
		}

		public void Enter(string sceneName)
		{
			_curtaine.Show();
			_factory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit()
		{
			_curtaine.Hide();
		}

		private void OnLoaded()
		{
			InitGameWorld();
			InformProgressReaders();

			_stateMachine.Enter<GameLoopState>();
		}

		private void InformProgressReaders()
		{
			foreach (ILoadProgress progressReader in _factory.ProgressReaders)
			{
				progressReader.LoadProgress(_progressService.Progress);
			}
		}

		private void InitGameWorld()
		{
			InitEnemySpawners();
			InitUnpickableLoot();
			
			GameObject knight = _factory.CreateKnight(GameObject.FindWithTag(InitialPointTag));
			
			GameObject hud = _factory.CreateHud();
			hud.GetComponentInChildren<ActorUI>().Constructor(knight.GetComponent<KnightHealth>());
			
			CameraFollow(knight);
		}

		private void InitEnemySpawners()
		{
			string sceneKey = SceneManager.GetActiveScene().name;
			LevelStaticData levelData = _staticData.ForLevel(sceneKey);
			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawner)
			{
				_factory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyType);
			}
		}

		private void InitUnpickableLoot()
		{
			foreach (LootSavePositionData lootSavePosition in _progressService.Progress.LootSavePositionData)
			{
				LootPiece loot = _factory.CreateLoot(lootSavePosition.LootPosition);
				LootData lootData = new LootData() { Value = lootSavePosition.Value };
				loot.Initialize(lootData);
			}
			_progressService.Progress.LootSavePositionData.Clear();
		}

		private static void CameraFollow(GameObject target)
		{
			Camera.main.GetComponent<CameraFollow>().Follow(target);
		}
	}
}