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
    public partial class ClientMainViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

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

        List<Product> AllProducts;
        
        public ClientMainViewModel(UserService userService, ProductService productService, CategoryService categoryService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            Task.Factory.StartNew(() =>
            {
                AllProducts = _productService.GetProducts().Result.ToList();
                Products = new ObservableCollection<Product>(AllProducts);
                Categories = new ObservableCollection<Category>(_categoryService.GetCategories().Result);
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

        [RelayCommand]
        void AddProductToCart(Product selectedProduct)
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
            }
            else
            {
                Cart.Add(new Cart(selectedProduct, 1));
            }
            CartUpdate();
        }

        [RelayCommand]
        void RemoveProductFromCart(Product selectedProduct)
        {
        }

        void CartUpdate()
        {
            _productService.Cart = Cart;
            CartPrice = Cart.Sum(x => (x.Product.Price - (x.Product.Price / 100 * (x.Product.Discount.HasValue ? x.Product.Discount.Value : 0))) * x.Count);
        }
    }
}