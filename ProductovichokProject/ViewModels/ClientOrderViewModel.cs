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
    public partial class ClientOrderViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly PageService _pageService;

        [ObservableProperty]
        ObservableCollection<Cart> allCart;

        [ObservableProperty]
        int? cartPrice = null;

        [ObservableProperty]
        string selectUserAddress;

        public ClientOrderViewModel(UserService userService, ProductService productService, PageService pageService)
        {
            _userService = userService;
            _productService = productService;
            _pageService = pageService;
            AllCart = _productService.Cart;
            CartUpdate();
            SelectUserAddress = $"ул. {_userService.SelectedUserAddress.Address.Street.StreetName}, дом {_userService.SelectedUserAddress.Address.HouseId}, квартира " + (_userService.SelectedUserAddress.Appartament.ToString() ?? "не указана") + ", подъезд " + (_userService.SelectedUserAddress.Entrance.ToString() ?? "не указан") + ", этаж " + _userService.SelectedUserAddress.Floor.ToString() ?? "не указан";
        }

        void CartUpdate()
        {
            _productService.Cart = AllCart;
            CartPrice = (int)AllCart.Sum(x => x.Product.DiscontPrice * x.Count);
        }

        [RelayCommand]
        async void CreateOrder()
        {
            Order order = new Order()
            {
                ClientId = _userService.UserInfo.UserId,
                StatusId = 1,
                AddressId = _userService.SelectedUserAddress.AddressId,
                TotalPrice = CartPrice,
                OrderDateTime = DateTime.Now,
                OrderComment = "ээ сука ебать нихуя"
            };
            await _productService.AddClientOrder(order);
        }

        [RelayCommand]
        async Task AddProductToCart(Cart selectedProduct)
        {
            var selectedProductInCart = AllCart.SingleOrDefault(x => x == selectedProduct);
            var index = AllCart.IndexOf(selectedProductInCart);
            if (selectedProduct.Product.Quantity > selectedProductInCart.Count) // проверка на кол-во продукта
            {

                selectedProductInCart.Count += 1;
                AllCart[index] = selectedProductInCart;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Товар кончился!", "Окей");
            }
            CartUpdate();
        }

        [RelayCommand]
        void RemoveProductFromCart(Cart selectedProduct)
        {
            if (AllCart.SingleOrDefault(x => x == selectedProduct).Count > 1)
            {
                AllCart.SingleOrDefault(x => x == selectedProduct).Count -= 1;
            }
            else
            {
                var selectedProductInCart = AllCart.SingleOrDefault(x => x == selectedProduct);
                AllCart.Remove(selectedProductInCart);
            }
            CartUpdate();
        }
    }
}
