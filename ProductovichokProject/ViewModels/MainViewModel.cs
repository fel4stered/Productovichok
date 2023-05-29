using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Services;
using ProductovichokProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductovichokProject.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly UserService _userService;

        public MainViewModel(UserService userService)
        {
            _userService = userService;
        }

        public ICommand LinkCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        [ObservableProperty]
        string nickname;
        [ObservableProperty]
        string code;
        [ObservableProperty]
        string error = "";

        [RelayCommand]
        async void SignIn()
        {
#if ANDROID
    await Shell.Current.GoToAsync(nameof(ClientMainPage));
#endif
            if (Nickname is not null)
            {
                if (int.TryParse(Code, out var CodeInt))
                {
                    if (await _userService.Authorization(Nickname, CodeInt))
                    {
                        await Shell.Current.GoToAsync(nameof(ClientMainPage));
                    }
                    else
                    {
                        Error = "Ошибка авторизации! Проверьте правильность написания логина и кода.";
                    }
                }
            }
        }
    }
}
