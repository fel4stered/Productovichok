﻿using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        ObservableCollection<Product> products;

        public ClientMainViewModel(UserService userService, ProductService productService)
        {
            _userService = userService;
            _productService = productService;
            Products = _productService.GetProducts().Result;
        }

        [RelayCommand]
        async void ScrollForward(ScrollView scrollView)
        {
            await scrollView.ScrollToAsync(scrollView.ScrollX + 20,0, true);
        }
    }
}