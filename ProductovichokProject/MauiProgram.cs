using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductovichokProject.Data;
using ProductovichokProject.Services;
using ProductovichokProject.ViewModels;
using ProductovichokProject.Views;
using ProductovichokProject.Views.Popups;

namespace ProductovichokProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #region Services
            #region ConnectDB
            var connectionString = "server=rc1b-kspwzb8gf9wxum7u.mdb.yandexcloud.net;user=kek;password=productovichok2116;database=productovichok; ";
            builder.Services.AddDbContext<ProductovichokContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            #endregion
            #region Pages and ViewModels
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<ClientMainPage>();
            builder.Services.AddTransient<ClientMainViewModel>();

            builder.Services.AddTransient<ClientOrderPage>();
            builder.Services.AddTransient<ClientOrderViewModel>();

            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProfileViewModel>();

            #endregion

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<PageService>();
            builder.Services.AddSingleton<OrderService>();
            #endregion

#if DEBUG
            builder.Logging.AddDebug();
#endif

            AllowMultiLineTruncationOnAndroid();

            return builder.Build();
        }
        static void AllowMultiLineTruncationOnAndroid()
        {
#if ANDROID

        /* 
		 * The default Controls handling of LineBreakMode and MaxLines on Android
		 * only allows single lines when using text truncation. However, combining
		 * setMaxLines() and TextUtils.TruncateAt.END _is_ supported on Android 
		 * (see https://developer.android.com/reference/android/widget/TextView#setEllipsize(android.text.TextUtils.TruncateAt))
		 * 
		 * The following code updates the mappings for Label on Android to support
		 * this scenario. Truncation and max lines both affect the platform setting
		 * of maximum lines, so we need to modify the mappings for both properties. 
		 * We append a second mapping that checks for our target situation (end truncation)
		 * and sets the maximum lines to the target value.
		*/

        static void UpdateMaxLines(Microsoft.Maui.Handlers.LabelHandler handler, ILabel label) 
		{
			var textView = handler.PlatformView;
			if (label is Label controlsLabel && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End)
			{
				textView.SetMaxLines(controlsLabel.MaxLines);
			}
		};

		Label.ControlsLabelMapper.AppendToMapping(
		   nameof(Label.LineBreakMode), UpdateMaxLines);

		Label.ControlsLabelMapper.AppendToMapping(
			nameof(Label.MaxLines), UpdateMaxLines);

#endif
        }

    }
}