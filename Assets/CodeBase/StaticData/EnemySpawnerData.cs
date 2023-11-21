using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	[Serializable]
	public class EnemySpawnerData
	{
		public string Id;
		public EnemyType EnemyType;
		public Vector3 Position;

		public EnemySpawnerData(string id, EnemyType type, Vector3 position)
		{
			Id = id;
			EnemyType = type;
			Position = position;
		}
	}
}