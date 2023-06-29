using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.ViewModels
{
    public partial class OrderDetailsViewModel : ObservableObject
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        [ObservableProperty]
        List<Orderdetail> orderDetails;
        [ObservableProperty]
        string selectUserAddress;
        [ObservableProperty]
        Order selectedOrder;

        public OrderDetailsViewModel(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
            SelectedOrder = _orderService.SelectedOrder;
            Task.Run(() =>
            {
                OrderDetails = _orderService.GetOrderDetails(_orderService.SelectedOrder.OrderId).Result;
            });
            UserAddress OrderAddress = _userService.UserAddressesAll.FirstOrDefault(x => x.AddressId == SelectedOrder.AddressId);
            string app = OrderAddress.Appartament.ToString() == string.Empty ? "не указана" : OrderAddress.Appartament.ToString().ToString();
            string pod = OrderAddress.Entrance.ToString().ToString() == string.Empty ? "не указан" : OrderAddress.Entrance.ToString().ToString();
            string floor = OrderAddress.Floor.ToString().ToString() == string.Empty ? "не указан" : OrderAddress.Floor.ToString().ToString();
            SelectUserAddress = $"ул. {SelectedOrder.Address.Street.StreetName}, дом {SelectedOrder.Address.HouseId}, квартира {app}, подъезд {pod}, этаж {floor}";
        }

        [RelayCommand]
        async void CheckRequest()
        {
            await Task.Run(async () => { await _orderService.CheckRequest(SelectedOrder.OrderId, _userService.UserInfo.UserId); });

            await Application.Current.MainPage.DisplayAlert("Запрос чека", "Запрос принят. Отправьте слово 'чек' нашему боту в телеграме и он пришлёт вам ваш чек.", "Окей");
        }
    }
}
