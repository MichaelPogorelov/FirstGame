using System;

namespace CodeBase.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public WorldData WorldData;
		public PlayerHealthData PlayerHealth;

		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			PlayerHealth = new PlayerHealthData();
		}
	}
}