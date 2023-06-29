using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;
using ProductovichokProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.ViewModels
{
    public partial class PickerMainViewModel : ObservableObject
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;
        private readonly PageService _pageService;

        [ObservableProperty]
        ObservableCollection<Order> orders;
        public PickerMainViewModel(OrderService orderService, UserService userService, PageService pageService)
        {
            _orderService = orderService;
            _userService = userService;
            _pageService = pageService;
            Task.Run(() => { Orders = _orderService.GetOrdersForPicker().Result; });
        }

        [RelayCommand]
        async void GoToOrderDetailsPage(Order order)
        {
            _orderService.SelectedOrder = order;
            await Task.Run(async () => { await _orderService.EditOrderStatus(2, order); });
            await _pageService.GoToPageAsync(nameof(PickerOrderDetailPage));
        }
    }
}
