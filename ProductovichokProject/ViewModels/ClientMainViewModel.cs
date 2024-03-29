﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductovichokProject.Data;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;
using ProductovichokProject.Views;
using ProductovichokProject.Views.Popups;
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
        private readonly ProductovichokContext _context;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly PageService _pageService;

        [ObservableProperty]
        ObservableCollection<Product> products;

        [ObservableProperty]
        ObservableCollection<Category> categories;

        [ObservableProperty]
        Category? selectedCategory;

        [ObservableProperty]
        int cartPrice = 0;

        [ObservableProperty]
        ObservableCollection<Cart> cart = new ObservableCollection<Cart>();

        [ObservableProperty]
        ObservableCollection<string> userAddressesShort = new ObservableCollection<string>() { "Добавить новый адрес" };

        [ObservableProperty]
        int addressPickerSelectedIndex = -1;

        [ObservableProperty]
        User userInfo;

        List<Product> AllProducts;
        List<UserAddress> UserAddresses;

        bool GoToProfilePageCheck = false;
        bool GoToOrderPageCheck = false;
        
        public ClientMainViewModel(UserService userService, ProductService productService, CategoryService categoryService, PageService pageService, ProductovichokContext context)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            _pageService = pageService;
            _context = context;
            UserInfo = _userService.UserInfo;
            Task.Factory.StartNew(() =>
            {
                AllProducts = _productService.GetProducts().Result.ToList();
                Products = new ObservableCollection<Product>(AllProducts);
                Categories = new ObservableCollection<Category>(_categoryService.GetCategories().Result);
                SelectedCategory = Categories[0];
                if (_userService.UserAddressesChecker().Result)
                {
                    UpdateUserAddresses();
                }
            });
        }

        async partial void OnSelectedCategoryChanging(Category? value)
        {
            await Task.Factory.StartNew(() =>
            {
                if (value.CategoryId == 1)
                {
                    Products = new ObservableCollection<Product>(AllProducts);
                    
                }
                else
                    Products = new ObservableCollection<Product>(AllProducts.Where(x => x.CategoryId == value.CategoryId));
            });
        }

        public void UpdateUserAddresses()
        {
            UserAddresses = _userService.GetUserAddresses().Result;
            _userService.UserAddressesAll = UserAddresses;
            UserAddressesShort = new ObservableCollection<string>(UserAddresses.Select(x => x.Address.Street.StreetName + ", " + x.Address.HouseId)) { "Добавить новый адрес" };
        }

        async partial void OnAddressPickerSelectedIndexChanged(int value)
        {
            if (value != -1 && UserAddressesShort[value] == "Добавить новый адрес")
            {
                var result = await Shell.Current.CurrentPage.ShowPopupAsync(new AddressPopup(_userService.UserInfo, UserAddresses,_context));
                if (result != null)
                {
                    await _userService.AddUserAddress((UserAddress)result);
                    await Task.Factory.StartNew(() => { UpdateUserAddresses(); });
                }
                else
                    AddressPickerSelectedIndex = 0;
            }
        }

        [RelayCommand]
        async Task AddProductToCart(Product selectedProduct)
        {
            if(Cart.Any(x => x.Product == selectedProduct)) // проверка на содержание в корзине
            {
                var selectedProductInCart = Cart.SingleOrDefault(x => x.Product == selectedProduct);
                var index = Cart.IndexOf(selectedProductInCart);
                if (selectedProduct.Quantity > selectedProductInCart.Count) // проверка на кол-во продукта
                {
                    
                    selectedProductInCart.Count += 1;
                    Cart[index] = selectedProductInCart;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "Товар кончился!", "Окей");
                }
            }
            else
            {
                Cart.Add(new Cart(selectedProduct, 1));
            }
            CartUpdate();
        }

        [RelayCommand]
        async void RemoveProductFromCart(Product selectedProduct)
        {
            if (!Cart.Any(x => x.Product == selectedProduct)) // проверка на содержание в корзине
            {
                await Application.Current.MainPage.DisplayAlert("","Товара нет в корзине!","Окей");
            }
            else if(Cart.SingleOrDefault(x => x.Product == selectedProduct).Count > 1)
            {
                Cart.SingleOrDefault(x => x.Product == selectedProduct).Count -= 1;
            }
            else
            {
                var selectedProductInCart = Cart.SingleOrDefault(x => x.Product == selectedProduct);
                Cart.Remove(selectedProductInCart);
            }
            CartUpdate();

        }

        void CartUpdate()
        {
            _productService.Cart = Cart;
            CartPrice = (int)Cart.Sum(x => x.Product.DiscontPrice * x.Count);
        }

        [RelayCommand]
        async void GoToOrderPage()
        {   
            if(AddressPickerSelectedIndex != -1)
            {
                if(GoToOrderPageCheck == false)
                {
                    GoToOrderPageCheck = true;
                    _userService.SelectedUserAddress = UserAddresses[AddressPickerSelectedIndex];
                    await _pageService.GoToPageAsync(nameof(ClientOrderPage));
                }
                
            }
            else
                await Application.Current.MainPage.DisplayAlert("", "Выберите адрес доставки!", "Окей");
        }

        [RelayCommand]
        void OnAppearing()
        {
            GoToProfilePageCheck = false;
            GoToOrderPageCheck = false;
            if (_productService.Cart is not null)
            {
                Cart = _productService.Cart;
                CartUpdate();
            }
        }

        [RelayCommand]
        async void GoToProfilePage()
        {
            if(GoToProfilePageCheck == false)
            {
                GoToProfilePageCheck = true;
                await _pageService.GoToPageAsync(nameof(ProfilePage));
            }
            
        }
    }
}