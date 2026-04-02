using DartMaster.ViewModels;

namespace DartMaster.Views;

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
