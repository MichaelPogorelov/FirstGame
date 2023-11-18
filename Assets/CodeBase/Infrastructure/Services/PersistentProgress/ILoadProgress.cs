using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
	public interface ILoadProgress : ISaveLoadProgress
	{
		void LoadProgress(PlayerProgress progress);
	}
}