using UnityEngine;

namespace CodeBase.Services.Input
{
	public abstract class InputService : IInputService
	{
		protected const string Horizontal = "Horizontal";
		protected const string Vertical = "Vertical";
		private const string Fire = "Fire";

		public abstract Vector2 Axis { get; }

		public bool IsAttackButtonUp()
		{
			return SimpleInput.GetButtonUp(Fire);
		}

		protected static Vector2 SimpleInputAxis()
		{
			return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
		}
	}
}