using System;

namespace CodeBase.Data
{
	[Serializable]
	public class LootSaveData
	{
		public int Collected;
		public event Action Changed;

		public void Collect(LootData lootData)
		{
			Collected += lootData.Value;
			Changed?.Invoke();
		}
		
		public void Add(int value)
		{
			Collected += value;
			Changed?.Invoke();
		}
	}
}