using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
	public class AdsService : IUnityAdsListener, IAdsService
	{
		public event Action RewardedVideoReady;
		public int Reward => 5;

		private string _platformID;
		private string _videoPlatformID;
		private Action _onVideoFinished;

		private const string AndroidID = "5483888";
		private const string AndroidRewardedVideoID = "Rewarded_Android";
		private const string IOSID = "5483889";
		private const string IOSRewardedVideoID = "Rewarded_iOS";


		public void Initialize()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.Android:
					_platformID = AndroidID;
					_videoPlatformID = AndroidRewardedVideoID;
					break;
				case RuntimePlatform.IPhonePlayer:
					_platformID = IOSID;
					_videoPlatformID = IOSRewardedVideoID;
					break;
				case RuntimePlatform.OSXEditor:
					_platformID = IOSID;
					_videoPlatformID = IOSRewardedVideoID;
					break;
				default:
					Debug.Log("Unsupported platform for Ads");
					break;
			}
			
			Advertisement.AddListener(this);
			Advertisement.Initialize(_platformID);
		}

		public void ShowRewardedVideo(Action onVideoFinish)
		{
			Advertisement.Show(_videoPlatformID);
			_onVideoFinished = onVideoFinish;
		}

		public bool IsRewardedVideoReady()
		{
			return Advertisement.IsReady(_videoPlatformID);
		}


		public void OnUnityAdsReady(string placementId)
		{
			Debug.Log($"OnUnityAdsReady {placementId}");
			if (placementId == _videoPlatformID)
			{
				RewardedVideoReady?.Invoke();
			}
		}

		public void OnUnityAdsDidError(string message)
		{
			Debug.Log($"{message}");
		}

		public void OnUnityAdsDidStart(string placementId)
		{
			Debug.Log($"OnUnityAdsDidStarted {placementId}");
		}

		public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
		{
			switch (showResult)
			{
				case ShowResult.Failed:
					Debug.LogError($"OnUnityAdsDidFinish {showResult}");
					break;
				case ShowResult.Finished:
					_onVideoFinished?.Invoke();
					break;
				case ShowResult.Skipped:
					Debug.LogError($"OnUnityAdsDidFinish {showResult}");
					break;
				default:
					Debug.LogError("Default show result");
					break;
			}

			_onVideoFinished = null;
		}
	}
}