using DartsCounter.ViewModels;

namespace DartsCounter.Views;

public partial class SetupPlayers : ContentPage
{
	private GameViewModel ViewModel => (GameViewModel)BindingContext;

	public SetupPlayers()
	{
		InitializeComponent();
	}

	private void OnAddPlayerClicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(PlayerNameEntry.Text))
		{
			ViewModel.AddPlayerCommand.Execute(PlayerNameEntry.Text);
			PlayerNameEntry.Text = string.Empty;
			PlayerNameEntry.Focus();
		}
	}

	private async void OnStartMatchClicked(object sender, EventArgs e)
	{
		if (ViewModel.Players.Count == 0)
		{
			await DisplayAlert("No Players", "Please add at least one player to start the match.", "OK");
			return;
		}

		// Navigate to ControllerPage and pass the current ViewModel
		if (Application.Current != null)
		{
			Application.Current.MainPage = new NavigationPage(new ControllerPage(ViewModel));
		}
	}
}
