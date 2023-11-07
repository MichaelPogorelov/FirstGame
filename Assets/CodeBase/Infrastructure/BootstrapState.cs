using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _stateMachine;

		public BootstrapState(GameStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public void Enter()
		{
			RegisterServices();
		}

		private void RegisterServices()
		{
			Game.InputService = ChooseInputService();
		}

		public void Exit()
		{
		}
		
		private static IInputService ChooseInputService()
		{
			if (Application.isEditor)
			{
				return new StandaloneInputService();
			}
			else
			{
				return new MobileInputService();
			}
		}
	}
}