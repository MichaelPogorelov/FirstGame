using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
	public class LootCounter : MonoBehaviour
	{
		public TextMeshProUGUI Counter;
		private WorldData _worldData;

		public void Constructor(WorldData worldData)
		{
			_worldData = worldData;
			_worldData.LootSaveData.Changed += UpdateCounter;
		}

		private void Start()
		{
			UpdateCounter();
		}

		private void UpdateCounter()
		{
			Counter.text = $"{_worldData.LootSaveData.Collected}";
		}
	}
}