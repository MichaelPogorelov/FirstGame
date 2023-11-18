using CodeBase.Loot;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class UnityRandomService : IRandomService
	{
		public int Next(int min, int max)
		{
			return Random.Range(min, max);
		}
	}
}