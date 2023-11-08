using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
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
			_stateMachine.Enter<LoadLevelState, string>("Main");
		}

		private void RegisterServices()
		{
			_services.RegisterSingle<IInputService>(ChooseInputService());
			_services.RegisterSingle<IAssetProvider>(new AssetProvider());
			_services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>()));
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