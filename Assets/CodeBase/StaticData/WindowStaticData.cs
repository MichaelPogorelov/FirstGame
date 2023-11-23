using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "WindowData", menuName = "StaticData/Window")]
	public class WindowStaticData : ScriptableObject
	{
		public List<WindowConfig> Windows = new List<WindowConfig>();
	}
}