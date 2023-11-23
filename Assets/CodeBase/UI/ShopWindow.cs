using TMPro;

namespace CodeBase.UI
{
	public class ShopWindow : WindowBase
	{
		public TextMeshProUGUI ScullCount;

		protected override void Initialize()
		{
			RefreshText();
		}

		protected override void SubscribeUpdates()
		{
			Progress.WorldData.LootSaveData.Changed += RefreshText;
		}

		protected override void Cleanup()
		{
			base.Cleanup();
			Progress.WorldData.LootSaveData.Changed -= RefreshText;
		}

		private void RefreshText()
		{
			ScullCount.text = Progress.WorldData.LootSaveData.Collected.ToString();
		}
	}
}