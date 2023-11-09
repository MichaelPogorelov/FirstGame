using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		List<ISaveProgressReader> ProgressReaders { get; }
		List<ISaveProgress> ProgressWriters { get; }
		GameObject CreateKnight(GameObject at);
		event Action KnightCreated;
		GameObject KnightGameObject { get; }
		void CreateHud();
		void Cleanup();
	}
}