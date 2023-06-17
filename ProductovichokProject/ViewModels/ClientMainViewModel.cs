using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        
        public ClientMainViewModel(UserService userService, ProductService productService, CategoryService categoryService, PageService pageService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            _pageService = pageService;
            UserInfo = _userService.UserInfo;
            Task.Factory.StartNew(() =>
            {
                AllProducts = _productService.GetProducts().Result.ToList();
                Products = new ObservableCollection<Product>(AllProducts);
                Categories = new ObservableCollection<Category>(_categoryService.GetCategories().Result);
                if (_userService.UserAddressesChecker().Result)
                {
                    UpdateUserAddresses();
                }
            });
            if (_productService.Cart is not null)
            {
                Cart = _productService.Cart;
            }
        }

        async partial void OnSelectedCategoryChanging(Category? value)
        {
            await Task.Factory.StartNew(() =>
            {
                if(value is not null)
                {
                    Products = new ObservableCollection<Product>(AllProducts.Where(x => x.CategoryId == value.CategoryId));
                }
            });
        }

        public void UpdateUserAddresses()
        {
            var UserAddresses = _userService.GetUserAddresses().Result;
            UserAddressesShort = new ObservableCollection<string>(UserAddresses.Select(x => x.Address.Street.StreetName + ", " + x.Address.HouseId)) { "Добавить новый адрес" };
        }

        async partial void OnAddressPickerSelectedIndexChanged(int value)
        {
            if (value != -1 && UserAddressesShort[value] == "Добавить новый адрес")
            {
                var result = await Shell.Current.CurrentPage.ShowPopupAsync(new AddressPopup(_userService.UserInfo));
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
        async Task AddProductToCartAsync(Product selectedProduct)
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
            await _pageService.GoToPageAsync(nameof(ClientOrderPage));
        }
    }
}