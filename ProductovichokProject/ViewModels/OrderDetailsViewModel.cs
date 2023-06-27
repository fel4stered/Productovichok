using CommunityToolkit.Mvvm.ComponentModel;
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
            Task.Run(async () => { OrderDetails = await _orderService.GetOrderDetails(_orderService.SelectedOrder.OrderId); });
            SelectUserAddress = $"ул. {_orderSe}, дом {_userService.SelectedUserAddress.Address.HouseId}, квартира " + (_userService.SelectedUserAddress.Appartament.ToString() ?? "не указана") + ", подъезд " + (_userService.SelectedUserAddress.Entrance.ToString() ?? "не указан") + ", этаж " + _userService.SelectedUserAddress.Floor.ToString() ?? "не указан";
        }
    }
}
