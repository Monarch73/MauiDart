using DartsCounter.ViewModels;

namespace DartsCounter.Views;

public partial class ControllerPage : ContentPage
{
	public ControllerPage()
	{
		InitializeComponent();
	}

	public ControllerPage(GameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
