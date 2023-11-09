using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.UI
{
	public class ActorUI : MonoBehaviour
	{
		public HPBar HpBar;
		private KnightHealth _knightHealth;

		public void Constructor(KnightHealth knightHealth)
		{
			_knightHealth = knightHealth;

			_knightHealth.HealthChange += UpdateHPBar;
		}

		private void UpdateHPBar()
		{
			HpBar.SetValue(_knightHealth.CurrentHP, _knightHealth.CurrentMaxHP);
		}

		private void OnDestroy()
		{
			_knightHealth.HealthChange -= UpdateHPBar;
		}
	}
}