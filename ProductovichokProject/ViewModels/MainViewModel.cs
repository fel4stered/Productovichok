using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Services;
using ProductovichokProject.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductovichokProject.ViewModels
{
    public partial class MainViewModel : ObservableObject 
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;

        public MainViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }

        public ICommand LinkCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        [ObservableProperty]
        string nickname;
        [ObservableProperty]
        string code;
        [ObservableProperty]
        string error = "";
        [ObservableProperty]
        bool loading = false;

        [RelayCommand]
        async void SignIn()
        {
            if (Nickname is not null)
            {
                if (int.TryParse(Code, out var CodeInt))
                {
                    Loading = true;
                    
                    if (await _userService.Authorization(Nickname, CodeInt))
                    {
                        await _pageService.GoToPageAsync(nameof(ClientMainPage));
                    }
                    else
                    {
                        Task.Delay(200).Wait();
                        Error = "Ошибка авторизации! Проверьте правильность написания логина и кода.";
                    }
                    Loading = false;
                }
            }
        }
    }
}
