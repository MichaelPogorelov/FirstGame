using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
	public class OpenWindowButton : MonoBehaviour
	{
		public Button Button;
		public WindowType Type;
		private IWindowService _windowService;

		public void Constructor(IWindowService windowService)
		{
			_windowService = windowService;
		}
		private void Awake()
		{
			Button.onClick.AddListener(Open);
		}

		private void Open()
		{
			_windowService.Open(Type);
		}
	}
}