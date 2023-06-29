using CommunityToolkit.Maui.Alerts;
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
            string app = _userService.SelectedUserAddress.Appartament.ToString() == string.Empty ? "не указана" : _userService.SelectedUserAddress.Appartament.ToString();
            string pod = _userService.SelectedUserAddress.Entrance.ToString() == string.Empty ? "не указан" : _userService.SelectedUserAddress.Entrance.ToString();
            string floor = _userService.SelectedUserAddress.Floor.ToString() == string.Empty ? "не указан" : _userService.SelectedUserAddress.Floor.ToString();
            SelectUserAddress = $"ул. {_userService.SelectedUserAddress.Address.Street.StreetName}, дом {_userService.SelectedUserAddress.Address.HouseId}, квартира {app}, подъезд {pod}, этаж {floor}";
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
            };
            await _productService.AddClientOrder(order);
            var toast = Toast.Make("Спасибо за заказ! Наши сотрудники уже начали его сборку!", CommunityToolkit.Maui.Core.ToastDuration.Long);
            await toast.Show();
            _productService.Cart = new ObservableCollection<Cart>();
            await _pageService.GoToPageAsync("..");
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
