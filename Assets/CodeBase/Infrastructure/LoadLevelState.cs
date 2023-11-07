using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private const string InitialPointTag = "InitialPoint";
		private const string KnightPath = "Hero/hero";
		private const string HudPath = "Hud/hud";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtaine;

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
			var initialPoint = GameObject.FindWithTag(InitialPointTag);
			
			GameObject knight = Instantiate(KnightPath, initialPoint.transform.position);
			Instantiate(HudPath);
			
			CameraFollow(knight);
			
			_stateMachine.Enter<GameLoopState>();
		}
		private static void CameraFollow(GameObject target)
		{
			Camera.main.GetComponent<CameraFollow>().Follow(target);
		}

		private static GameObject Instantiate(string path)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}
		
		private static GameObject Instantiate(string path, Vector3 at)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}
	}
}