using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		GameObject CreateKnight(GameObject at);
		void CreateHud();
	}
}