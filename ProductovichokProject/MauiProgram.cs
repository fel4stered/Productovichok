using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductovichokProject.Data;
using ProductovichokProject.Services;
using ProductovichokProject.ViewModels;
using ProductovichokProject.Views;

namespace ProductovichokProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            #region ConnectDB
            var connectionString = "server=172.17.142.180;user=root;password=1234;database=productovichok";
            var services = new ServiceCollection();
            services.AddDbContext<ProductovichokContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            #endregion
            
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

            #region Pages and ViewModels
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<ClientMainPage>();
            builder.Services.AddTransient<ClientMainViewModel>(); 
            #endregion

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ProductovichokContext>();
            builder.Services.AddSingleton<ProductService>();
            #endregion

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}