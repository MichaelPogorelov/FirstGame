using System;

namespace CodeBase.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public WorldData WorldData;
		public PlayerHealthData PlayerHealth;
		public PlayerAttackData PlayerAttack;

		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			PlayerHealth = new PlayerHealthData();
			PlayerAttack = new PlayerAttackData();
		}
	}
}