using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

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

		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtaine, IGameFactory factory, IPersistentProgressService progressService)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtaine = curtaine;
			_factory = factory;
			_progressService = progressService;
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
			foreach (ISaveProgressReader progressReader in _factory.ProgressReaders)
			{
				progressReader.LoadProgress(_progressService.Progress);
			}
		}

		private void InitGameWorld()
		{
			InitEnemySpawners();
			
			GameObject knight = _factory.CreateKnight(GameObject.FindWithTag(InitialPointTag));
			
			GameObject hud = _factory.CreateHud();
			hud.GetComponentInChildren<ActorUI>().Constructor(knight.GetComponent<KnightHealth>());
			
			CameraFollow(knight);
		}

		private void InitEnemySpawners()
		{
			foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
			{
				var spawner = spawnerObject.GetComponent<EnemySpawner>();
				_factory.Register(spawner);
			}
		}

		private static void CameraFollow(GameObject target)
		{
			Camera.main.GetComponent<CameraFollow>().Follow(target);
		}
	}
}