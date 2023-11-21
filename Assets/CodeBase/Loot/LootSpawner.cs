using CodeBase.Data;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Loot
{
	public class LootSpawner : MonoBehaviour
	{
		public EnemyDeath enemyDeath;
		private IGameFactory _gameFactory;
		private int _lootMin;
		private int _lootMax;
		private IRandomService _random;

		public void Constructor(IGameFactory gameFactory, IRandomService random)
		{
			_gameFactory = gameFactory;
			_random = random;
		}
		private void Start()
		{
			enemyDeath.DeathHappend += SpawnLoot;
		}

		public void SetLoot(int min, int max)
		{
			_lootMin = min;
			_lootMax = max;
		}

		private void SpawnLoot()
		{
			LootPiece loot = _gameFactory.CreateLoot(transform.position);
			var lootItem = new LootData()
			{
				Value = _random.Next(_lootMin, _lootMax)
			};
			
			loot.Initialize(lootItem);
		}
	}
}