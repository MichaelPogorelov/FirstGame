using CodeBase.Infrastructure.AssetManadgment;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class GameFactory : IGameFactory
	{
		private readonly IAssetProvider _assets;

		public GameFactory(IAssetProvider assets)
		{
			_assets = assets;
		}

		public GameObject CreateKnight(GameObject at)
		{
			return _assets.Instantiate(AssetPath.KnightPath, at.transform.position);
		}

		public void CreateHud()
		{
			_assets.Instantiate(AssetPath.HudPath);
		}
	}
}