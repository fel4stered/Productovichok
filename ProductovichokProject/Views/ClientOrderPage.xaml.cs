using ProductovichokProject.ViewModels;

namespace ProductovichokProject.Views;

public partial class ClientOrderPage : ContentPage
{
	public ClientOrderPage(ClientOrderViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}