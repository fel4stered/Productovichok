using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Data.Models
{
    public partial class Cart : ObservableObject
    {
        public Product Product { get; set; }
        [ObservableProperty]
        public int count;

        partial void OnCountChanging(int value)
        {
            SumPrice = value * Product.Price;
            SumDiscountPrice = value * Product.DiscontPrice;
        }
        [ObservableProperty]
        int? sumPrice;
        [ObservableProperty]
        int? sumDiscountPrice;

        public Cart(Product product, int count)
        {
            Product = product;
            Count = count;
        }
    }
}
