using ProductovichokProject.ViewModels;

namespace ProductovichokProject.Views;

public partial class ClientMainPage : ContentPage
{
	public ClientMainPage(ClientMainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}