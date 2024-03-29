﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;
using ProductovichokProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        [ObservableProperty]
        List<Order> orders = new List<Order>();
        [ObservableProperty]
        string userName;

        bool isGoTo;

        public ProfileViewModel(UserService userService, PageService pageService, ProductService productService, OrderService orderService)
        {
            _userService = userService;
            _pageService = pageService;
            _productService = productService;
            _orderService = orderService;
            Task.Run(() => { Orders = _orderService.GetOrders(_userService.UserInfo.UserId).Result; });
            UserName = _userService.UserInfo.TelegramUserNickname;
        }

        [RelayCommand]
        async void GoToOrderDetailsPage(Order order)
        {
            if(isGoTo == false)
            {
                isGoTo = true;
                _orderService.SelectedOrder = order;
                await _pageService.GoToPageAsync(nameof(OrderDetailsPage));
            }
        }

        [RelayCommand]
        void OnAppearing()
        {
            isGoTo = false;
        }
    }
}
