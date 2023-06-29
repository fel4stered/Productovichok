using ProductovichokProject.ViewModels;

namespace ProductovichokProject.Views;

public partial class PickerOrderDetailPage : ContentPage
{
	public PickerOrderDetailPage(PickerOrderDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}