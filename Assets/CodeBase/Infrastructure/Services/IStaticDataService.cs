using CodeBase.Logic;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Services
{
	public interface IStaticDataService : IService
	{
		void LoadEnemy();
		EnemyStaticData ForEnemy(EnemyType type);
		LevelStaticData ForLevel(string sceneName);
	}
}