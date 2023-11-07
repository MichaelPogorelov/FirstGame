using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string InitialPointTag = "InitialPoint";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtaine;
		private readonly IGameFactory _factory;

		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtaine)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtaine = curtaine;
		}

		public void Enter(string sceneName)
		{
			_curtaine.Show();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit()
		{
			_curtaine.Hide();
		}

		private void OnLoaded()
		{
			GameObject knight = _factory.CreateKnight(GameObject.FindWithTag(InitialPointTag));

			_factory.CreateHud();
			
			CameraFollow(knight);
			
			_stateMachine.Enter<GameLoopState>();
		}

		private static void CameraFollow(GameObject target)
		{
			Camera.main.GetComponent<CameraFollow>().Follow(target);
		}
	}
}