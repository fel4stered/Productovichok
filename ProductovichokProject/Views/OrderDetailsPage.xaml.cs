using ProductovichokProject.ViewModels;

namespace ProductovichokProject.Views;

public partial class OrderDetailsPage : ContentPage
{
	public OrderDetailsPage(OrderDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}