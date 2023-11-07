using UnityEngine;

namespace CodeBase.Infrastructure
{
	public interface IGameFactory
	{
		GameObject CreateKnight(GameObject at);
		void CreateHud();
	}
}