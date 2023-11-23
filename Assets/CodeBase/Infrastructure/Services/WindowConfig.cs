using System;
using CodeBase.UI;
using CodeBase.UI.Services;

namespace CodeBase.Infrastructure.Services
{
	[Serializable]
	public class WindowConfig
	{
		public WindowType Type;
		public WindowBase Prefab;
	}
}