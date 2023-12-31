using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
	[CustomEditor(typeof(LevelStaticData))]
	public class LevelStaticDataEditor : UnityEditor.Editor
	{
		private const string InitialPointTag = "InitialPoint";
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			LevelStaticData levelData = (LevelStaticData)target;
			if (GUILayout.Button("Collect"))
			{
				levelData.EnemySpawner = FindObjectsOfType<SpawnMarker>().Select(x =>
					new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.EnemyType, x.transform.position)).ToList();

				levelData.LevelKey = SceneManager.GetActiveScene().name;

				levelData.InitialPlayerPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
			}
			
			EditorUtility.SetDirty(target);
		}
	}
}