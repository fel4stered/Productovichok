using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.ViewModels
{
    public partial class ClientMainViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        [ObservableProperty]
        ObservableCollection<Product> products;

        [ObservableProperty]
        ObservableCollection<Category> categories;

        public ClientMainViewModel(UserService userService, ProductService productService, CategoryService categoryService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            Products = _productService.GetProducts().Result;
            Categories = new ObservableCollection<Category>(_categoryService.GetCategories().Result);
        }

        [RelayCommand]
        async void ScrollForward(ScrollView scrollView)
        {
            await scrollView.ScrollToAsync(scrollView.ScrollX + 20, 0, true);
        }
    }
}