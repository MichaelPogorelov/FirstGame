using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
	[Serializable]
	public class EnemyDeathData
	{
		public List<string> ClearedSpawners = new List<string>();
	}
}