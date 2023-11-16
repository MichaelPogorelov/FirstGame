using CodeBase.Infrastructure.Services;

namespace CodeBase.Loot
{
	public interface IRandomService : IService
	{
		int Next(int min, int max);
	}
}