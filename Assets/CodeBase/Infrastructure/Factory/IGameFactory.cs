using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Loot;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		List<ISaveProgressReader> ProgressReaders { get; }
		List<ISaveProgress> ProgressWriters { get; }
		GameObject CreateKnight(GameObject at);
		GameObject CreateHud();
		void Cleanup();

		void Register(ISaveProgressReader progress);
		GameObject CreateEnemy(EnemyType type, Transform transform);
		LootPiece CreateLoot();
	}
}