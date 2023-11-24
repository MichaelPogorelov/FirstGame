using System;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.Ads
{
	public interface IAdsService : IService
	{
		event Action RewardedVideoReady;
		int Reward { get; }
		void Initialize();
		void ShowRewardedVideo(Action onVideoFinish);
		bool IsRewardedVideoReady();
	}
}