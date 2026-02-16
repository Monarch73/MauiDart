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
			var window = new Window(new NavigationPage(new Views.SetupPlayers()));
			
			// Set height to full screen resolution height
			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			window.Height = displayInfo.Height / displayInfo.Density;
			window.Y = 0; // Position at the top

			return window;
		}
		else
		{
			return new Window(new Views.DisplayPage());
		}
	}
}