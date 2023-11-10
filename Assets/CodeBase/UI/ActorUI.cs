using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
	public class ActorUI : MonoBehaviour
	{
		public HPBar HpBar;
		private IHealth _knightHealth;

		public void Constructor(IHealth knightHealth)
		{
			_knightHealth = knightHealth;

			_knightHealth.HealthChanged += UpdateHPBar;
		}

		private void UpdateHPBar()
		{
			HpBar.SetValue(_knightHealth.CurrentHP, _knightHealth.MaxHP);
		}

		private void OnDestroy()
		{
			_knightHealth.HealthChanged -= UpdateHPBar;
		}
	}
}