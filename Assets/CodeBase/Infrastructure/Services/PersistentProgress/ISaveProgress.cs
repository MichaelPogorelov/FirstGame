using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
	public interface ISaveProgress : ISaveLoadProgress
	{
		void SaveProgress(PlayerProgress progress);
	}
}