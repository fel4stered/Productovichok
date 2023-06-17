using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Data.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Count { get; set; }
        public int? SumPrice
        {
            get 
            { 
                return Product.Price * Count;
            }
        }
        public int? SumDiscountPrice
        {
            get
            {
                return Product.DiscontPrice * Count;
            }
        }

        public Cart(Product product, int count)
        {
            Product = product;
            Count = count;
        }
    }
}
