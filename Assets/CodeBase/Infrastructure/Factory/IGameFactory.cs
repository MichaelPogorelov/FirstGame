using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		GameObject CreateKnight(GameObject at);
		void CreateHud();
		List<ISaveProgressReader> ProgressReaders { get; }
		List<ISaveProgress> ProgressWriters { get; }
		void Cleanup();
	}
}