using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class Game
	{
		public static IInputService InputService;

		public Game()
		{
			ChooseInputService();
		}

		private static void ChooseInputService()
		{
			if (Application.isEditor)
			{
				InputService = new StandaloneInputService();
			}
			else
			{
				InputService = new MobileInputService();
			}
		}
	}
}