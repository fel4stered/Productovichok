using ProductovichokProject.Views;
using ProductovichokProject.Views.Popups;

namespace ProductovichokProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ClientMainPage), typeof(ClientMainPage));
            Routing.RegisterRoute(nameof(ClientOrderPage), typeof(ClientOrderPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            Routing.RegisterRoute(nameof(PickerMainPage), typeof(PickerMainPage));
            Routing.RegisterRoute(nameof(PickerOrderDetailPage), typeof(PickerOrderDetailPage));
        }
    }
}