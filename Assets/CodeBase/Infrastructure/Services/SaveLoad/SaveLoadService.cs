using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
	public class SaveLoadService : ISaveLoadService
	{
		private readonly IGameFactory _factory;
		private readonly IPersistentProgressService _progressService;
		private const string ProgressKey = "Progress";

		public SaveLoadService(IPersistentProgressService progressService, IGameFactory factory)
		{
			_progressService = progressService;
			_factory = factory;
		}
		public void SaveProgress()
		{
			_progressService.Progress.LootSavePositionData.Clear();
			
			foreach (ISaveProgress progressWriter in _factory.ProgressWriters)
			{
				progressWriter.SaveProgress(_progressService.Progress);
			}
			
			PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
		}

		public PlayerProgress LoadProgress()
		{
			return PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
		}
	}
}