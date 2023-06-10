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

        public ClientMainViewModel(UserService userService, ProductService productService, CategoryService categoryService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            Task.Factory.StartNew(() =>
            {
                Products = _productService.GetProducts().Result;
                Categories = new ObservableCollection<Category>(_categoryService.GetCategories().Result);
            });
        }

        partial void OnSelectedCategoryChanged(Category? value)
        {
            Task.Factory.StartNew(() =>
            {
                if(value is not null)
                {
                    Products = new ObservableCollection<Product>(_productService.GetProducts().Result.Where(x => x.CategoryId == value.CategoryId));
                }
            });
        }
    }
}