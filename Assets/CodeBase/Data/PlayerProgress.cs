using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public WorldData WorldData;
		public PlayerHealthData PlayerHealth;
		public PlayerAttackData PlayerAttack;
		public EnemyDeathData EnemyDeath;
		public PurchaseProductData PurchaseProduct;
		public List<LootSavePositionData> LootSavePositionData;

		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			PlayerHealth = new PlayerHealthData();
			PlayerAttack = new PlayerAttackData();
			EnemyDeath = new EnemyDeathData();
			PurchaseProduct = new PurchaseProductData();
			LootSavePositionData = new List<LootSavePositionData>();
		}
	}
}