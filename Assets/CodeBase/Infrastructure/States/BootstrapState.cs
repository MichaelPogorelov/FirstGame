using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Loot;
using CodeBase.Services.Ads;
using CodeBase.Services.IAP;
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
			RegisterAds();
			RegisterAssetProvider();

			_services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
			
			RegisterIAP(new IAPProvider(), _services.Single<IPersistentProgressService>());
			
			_services.RegisterSingle<IGameStateMachine>(_stateMachine);
			_services.RegisterSingle<IRandomService>(new UnityRandomService());
			_services.RegisterSingle<IInputService>(ChooseInputService());
			_services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(), _services.Single<IAdsService>()));
			_services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IRandomService>(), _services.Single<IPersistentProgressService>(), _services.Single<IWindowService>()));
			_services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
		}

		private void RegisterAssetProvider()
		{
			AssetProvider assetProvider = new AssetProvider();
			assetProvider.Initialize();
			_services.RegisterSingle<IAssetProvider>(assetProvider);
		}

		private void RegisterStaticData()
		{
			IStaticDataService staticData = new StaticDataService();
			staticData.LoadStaticData();
			_services.RegisterSingle<IStaticDataService>(staticData);
		}

		private void RegisterAds()
		{
			var adsService = new AdsService();
			adsService.Initialize();
			_services.RegisterSingle<IAdsService>(adsService);
		}

		private void RegisterIAP(IAPProvider iapProvider, IPersistentProgressService progressService)
		{
			var iapService = new IAPService(iapProvider, progressService);
			iapService.Initialize();
			_services.RegisterSingle<IIAPService>(iapService);
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