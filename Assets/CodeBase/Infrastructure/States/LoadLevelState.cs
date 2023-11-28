using System.Threading.Tasks;
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
using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtaine;
		private readonly IGameFactory _factory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticData;
		private readonly IUIFactory _uiFactory;

		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtaine, 
			IGameFactory factory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtaine = curtaine;
			_factory = factory;
			_progressService = progressService;
			_staticData = staticData;
			_uiFactory = uiFactory;
		}

		public void Enter(string sceneName)
		{
			_curtaine.Show();
			_factory.Cleanup();
			_factory.Warmup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit()
		{
			_curtaine.Hide();
		}

		private async void OnLoaded()
		{
			InitUIRoot();
			await InitGameWorld();
			InformProgressReaders();

			_stateMachine.Enter<GameLoopState>();
		}

		private void InitUIRoot()
		{
			_uiFactory.CreateUIRoot();
		}

		private void InformProgressReaders()
		{
			foreach (ILoadProgress progressReader in _factory.ProgressReaders)
			{
				progressReader.LoadProgress(_progressService.Progress);
			}
		}

		private async Task InitGameWorld()
		{
			LevelStaticData levelData = _staticData.ForLevel(SceneManager.GetActiveScene().name);
			
			await InitEnemySpawners(levelData);
			await InitUnpickableLoot();
			
			GameObject knight = _factory.CreateKnight(levelData.InitialPlayerPosition);
			
			GameObject hud = _factory.CreateHud();
			hud.GetComponentInChildren<ActorUI>().Constructor(knight.GetComponent<KnightHealth>());
			
			CameraFollow(knight);
		}

		private async Task InitEnemySpawners(LevelStaticData levelData)
		{
			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawner)
			{
				await _factory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyType);
			}
		}

		private async Task InitUnpickableLoot()
		{
			foreach (LootSavePositionData lootSavePosition in _progressService.Progress.LootSavePositionData)
			{
				LootPiece loot = await _factory.CreateLoot(lootSavePosition.LootPosition);
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