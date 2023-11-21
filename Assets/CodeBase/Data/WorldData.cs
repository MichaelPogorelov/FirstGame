using System;

namespace CodeBase.Data
{
	[Serializable]
	public class WorldData
	{
		public PositionOnLevel PositionOnLevel;
		public LootSaveData LootSaveData;

		public WorldData(string initialLevel)
		{
			PositionOnLevel = new PositionOnLevel(initialLevel);
			LootSaveData = new LootSaveData();
		}
	}
}