using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Loot;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";
		private readonly GameStateMachine _stateMachine;
		private SceneLoader _sceneLoader;
		private AllServices _services;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_services = services;
			
			RegisterServices();
		}

		public void Enter()
		{
			_sceneLoader.Load(Initial, EnterLoadLevel);
		}

		public void Exit()
		{
		}

		private void EnterLoadLevel()
		{
			_stateMachine.Enter<LoadProgressState>();
		}

		private void RegisterServices()
		{
			RegisterStaticData();
			
			_services.RegisterSingle<IRandomService>(new UnityRandomService());
			_services.RegisterSingle<IInputService>(ChooseInputService());
			_services.RegisterSingle<IAssetProvider>(new AssetProvider());
			_services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
			_services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>()));
			_services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IRandomService>(), _services.Single<IPersistentProgressService>(), _services.Single<IWindowService>()));
			_services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
		}

		private void RegisterStaticData()
		{
			IStaticDataService staticData = new StaticDataService();
			staticData.LoadStaticData();
			_services.RegisterSingle<IStaticDataService>(staticData);
		}

		private static IInputService ChooseInputService()
		{
			if (Application.isEditor)
			{
				return new StandaloneInputService();
			}
			else
			{
				return new MobileInputService();
			}
		}
	}
}