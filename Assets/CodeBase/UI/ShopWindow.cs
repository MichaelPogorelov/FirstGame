using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using TMPro;

namespace CodeBase.UI
{
	public class ShopWindow : WindowBase
	{
		public TextMeshProUGUI ScullCount;
		public RewardedAdItem AdItem;

		public void Constructor(IAdsService adsService, IPersistentProgressService progressService)
		{
			base.Constructor(progressService);
			AdItem.Constructor(adsService, progressService);
		}
		protected override void Initialize()
		{
			AdItem.Initialize();
			RefreshText();
		}

		protected override void SubscribeUpdates()
		{
			AdItem.Subscribe();
			Progress.WorldData.LootSaveData.Changed += RefreshText;
		}

		protected override void Cleanup()
		{
			base.Cleanup();
			AdItem.Cleanup();
			Progress.WorldData.LootSaveData.Changed -= RefreshText;
		}

		private void RefreshText()
		{
			ScullCount.text = Progress.WorldData.LootSaveData.Collected.ToString();
		}
	}
}