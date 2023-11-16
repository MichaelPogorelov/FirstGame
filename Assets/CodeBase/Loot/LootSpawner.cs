using System;
using CodeBase.Enemy.Lich;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Loot
{
	public class LootSpawner : MonoBehaviour
	{
		public LichDeath LichDeath;
		private IGameFactory _gameFactory;

		public void Constructor(IGameFactory gameFactory)
		{
			_gameFactory = gameFactory;
		}
		private void Start()
		{
			LichDeath.DeathHappend += SpawnLoot;
		}

		private void SpawnLoot()
		{
			GameObject loot = _gameFactory.CreateLoot();
			loot.transform.position = transform.position;
		}
	}
}