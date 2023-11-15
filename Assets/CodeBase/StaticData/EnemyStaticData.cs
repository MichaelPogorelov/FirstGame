using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
	public class EnemyStaticData : ScriptableObject
	{
		public EnemyType EnemyType;
		[Range(1, 100)] public int HP;
		[Range(1f, 100f)] public float Damage;
		[Range(0.5f, 5f)] public float AttackCooldown = 1f;
		[Range(0.1f, 2f)] public float HitRadius = 0.5f;
		[Range(0.1f, 2f)] public float ForwardDistanceCoef = 0.5f;
		[Range(0.5f, 7f)]public float MoveSpeed = 3f;

		public GameObject Prefab;
	}
}