using Microsoft.Maui.Platform;

namespace ProductovichokProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int minWidth = 1200;
            const int minHeight = 930;

            window.MinimumHeight = minHeight;
            window.MinimumWidth = minWidth;

            return window;
        }
    }
}