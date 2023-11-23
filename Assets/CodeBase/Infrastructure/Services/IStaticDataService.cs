using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Services;

namespace CodeBase.Infrastructure.Services
{
	public interface IStaticDataService : IService
	{
		void LoadStaticData();
		EnemyStaticData ForEnemy(EnemyType type);
		LevelStaticData ForLevel(string sceneName);
		WindowConfig ForWindow(WindowType type);
	}
}