using CommunityToolkit.Mvvm.ComponentModel;
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
        ObservableCollection<Cart> cart;

        public ClientOrderViewModel(UserService userService, ProductService productService, PageService pageService)
        {
            _userService = userService;
            _productService = productService;
            _pageService = pageService;
            Cart = _productService.Cart;
        }
    }
}
