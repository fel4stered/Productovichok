using ProductovichokProject.ViewModels;

namespace ProductovichokProject.Views;

public partial class PickerMainPage : ContentPage
{
	public PickerMainPage(PickerMainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}