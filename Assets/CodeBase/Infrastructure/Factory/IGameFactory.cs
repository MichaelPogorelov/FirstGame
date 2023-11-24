using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Loot;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		List<ILoadProgress> ProgressReaders { get; }
		List<ISaveProgress> ProgressWriters { get; }
		GameObject CreateKnight(Vector3 at);
		GameObject CreateHud();
		void Cleanup();
		Task<GameObject> CreateEnemy(EnemyType type, Transform transform);
		LootPiece CreateLoot(Vector3 position);
		void CreateSpawner(Vector3 at, string id, EnemyType type);
	}
}