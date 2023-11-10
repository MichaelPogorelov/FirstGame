using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		public List<ISaveProgressReader> ProgressReaders { get; } = new List<ISaveProgressReader>();
		public List<ISaveProgress> ProgressWriters { get; } = new List<ISaveProgress>();

		public event Action KnightCreated;
		public GameObject KnightGameObject { get; private set; }
		
		private readonly IAssetProvider _assets;


		public GameFactory(IAssetProvider assets)
		{
			_assets = assets;
		}

		public GameObject CreateKnight(GameObject at)
		{
			KnightGameObject = InstantiateRegister(AssetPath.KnightPath, at.transform.position);
			KnightCreated?.Invoke();
			return KnightGameObject;
		}

		public GameObject CreateHud()
		{
			return InstantiateRegister(AssetPath.HudPath);
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		private GameObject InstantiateRegister(string prefabPath, Vector3 at)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath, at);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}
		
		private GameObject InstantiateRegister(string prefabPath)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private void RegisterProgressWatchers(GameObject gameObject)
		{
			foreach (ISaveProgressReader progressReader in gameObject.GetComponentsInChildren<ISaveProgressReader>())
			{
				Register(progressReader);
			}
		}

		private void Register(ISaveProgressReader progressReader)
		{
			if (progressReader is ISaveProgress progressWriters)
			{
				ProgressWriters.Add(progressWriters);
			}
			ProgressReaders.Add(progressReader);
		}
	}
}