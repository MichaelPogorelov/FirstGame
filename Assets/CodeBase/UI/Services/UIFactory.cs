using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using UnityEngine;

namespace CodeBase.UI.Services
{
	public class UIFactory : IUIFactory
	{
		private const string UIRootPath = "UIRoot";
		private readonly IAssetProvider _assets;
		private readonly IStaticDataService _staticData;
		private Transform _uiRoot;
		private readonly IPersistentProgressService _progressService;
		private readonly IAdsService _adsService;

		public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService)
		{
			_assets = assets;
			_staticData = staticData;
			_progressService = progressService;
			_adsService = adsService;
		}

		public void CreateShop()
		{
			WindowConfig config = _staticData.ForWindow(WindowType.Shop);
			ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
			window.Constructor(_adsService, _progressService);
		}

		public async Task CreateUIRoot()
		{
			GameObject root = await _assets.Instantiate(UIRootPath);
			_uiRoot = root.transform;
		}
	}
}