namespace DartsCounter;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			return new Window(new NavigationPage(new Views.ControllerPage()));
		}
		else
		{
			return new Window(new Views.DisplayPage());
		}
	}
}