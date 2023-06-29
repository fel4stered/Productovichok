using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class PickerOrderDetailViewModel : ObservableObject
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;
        private readonly PageService _pageService;

        [ObservableProperty]
        List<Orderdetail> orderDetails;
        [ObservableProperty]
        string selectUserAddress;
        [ObservableProperty]
        Order selectedOrder;
        [ObservableProperty]
        string boxNumber;

        public PickerOrderDetailViewModel(OrderService orderService, UserService userService, PageService pageService)
        {
            _orderService = orderService;
            _userService = userService;
            _pageService = pageService;
            SelectedOrder = _orderService.SelectedOrder;
            Task.Run(() =>
            {
                OrderDetails = _orderService.GetOrderDetails(_orderService.SelectedOrder.OrderId).Result;
            });
            string app = SelectedOrder.Address.UserAddresses.ToList()[0].Appartament.ToString() == string.Empty ? "не указана" : SelectedOrder.Address.UserAddresses.ToList()[0].Appartament.ToString();
            string pod = SelectedOrder.Address.UserAddresses.ToList()[0].Entrance.ToString() == string.Empty ? "не указан" : SelectedOrder.Address.UserAddresses.ToList()[0].Entrance.ToString();
            string floor = SelectedOrder.Address.UserAddresses.ToList()[0].Floor.ToString() == string.Empty ? "не указан" : SelectedOrder.Address.UserAddresses.ToList()[0].Floor.ToString();
            SelectUserAddress = $"ул. {SelectedOrder.Address.Street.StreetName}, дом {SelectedOrder.Address.HouseId}, квартира {app}, подъезд {pod}, этаж {floor}";
        }

        [RelayCommand]
        async void TransferToDelivery()
        {
            await Task.Run(async () =>
            {
                await _orderService.EditOrderStatus(3, SelectedOrder, Int32.Parse(BoxNumber));
            });
            var toast = Toast.Make("Заказ передан в доставку.", CommunityToolkit.Maui.Core.ToastDuration.Long);
            await toast.Show();

            await _pageService.GoToPageAsync(nameof(PickerMainPage));
        }
    }
}
