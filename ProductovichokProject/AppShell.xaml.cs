using ProductovichokProject.Views;

namespace ProductovichokProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ClientMainPage), typeof(ClientMainPage));
        }
    }
}