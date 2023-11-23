namespace CodeBase.UI.Services
{
	public class WindowService : IWindowService
	{
		private readonly IUIFactory _uiFactory;
		
		public WindowService(IUIFactory uiFactory)
		{
			_uiFactory = uiFactory;
		}

		public void Open(WindowType windowType)
		{
			switch (windowType)
			{
				case WindowType.Unknow:
					break;
				case WindowType.Shop:
					_uiFactory.CreateShop();
					break;
			}
		}
	}
}