using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Services
{
	public class UIFactory : IUIFactory
	{
		private const string UIRootPath = "UI/UIRoot";
		private readonly IAssetProvider _assets;
		private readonly IStaticDataService _staticData;
		private Transform _uiRoot;
		private readonly IPersistentProgressService _progressService;

		public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
		{
			_assets = assets;
			_staticData = staticData;
			_progressService = progressService;
		}

		public void CreateShop()
		{
			WindowConfig config = _staticData.ForWindow(WindowType.Shop);
			WindowBase window = Object.Instantiate(config.Prefab, _uiRoot);
			window.Constructor(_progressService);
		}

		public void CreateUIRoot()
		{
			_uiRoot = _assets.Instantiate(UIRootPath).transform;
		}
	}
}