using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
	public class RewardedAdItem : MonoBehaviour
	{
		public Button ShowAdButton;
		public GameObject[] AdActiveObjects;
		public GameObject[] AdInactiveObjects;
		
		private IAdsService _adsService;
		private IPersistentProgressService _progressService;

		public void Constructor(IAdsService adsService, IPersistentProgressService progressService)
		{
			_adsService = adsService;
			_progressService = progressService;
		}
		
		public void Initialize()
		{
			ShowAdButton.onClick.AddListener(OnShowAdClicked);

			RefreshAvailableAd();
		}

		public void Subscribe()
		{
			_adsService.RewardedVideoReady += RefreshAvailableAd;
		}

		public void Cleanup()
		{
			_adsService.RewardedVideoReady -= RefreshAvailableAd;
		}

		private void OnShowAdClicked()
		{
			_adsService.ShowRewardedVideo(OnVideoFinish);
		}

		private void OnVideoFinish()
		{
			_progressService.Progress.WorldData.LootSaveData.Add(_adsService.Reward);
		}

		private void RefreshAvailableAd()
		{
			bool videoReady = _adsService.IsRewardedVideoReady();

			foreach (GameObject activeObject in AdActiveObjects)
			{
				activeObject.SetActive(videoReady);
			}

			foreach (GameObject inactiveObject in AdInactiveObjects)
			{
				inactiveObject.SetActive(!videoReady);
			}
		}

	}
}