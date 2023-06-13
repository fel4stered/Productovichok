using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Data.Models
{
    public class Cart
    {
        public Product Product;
        public int Count;

        public Cart(Product product, int count)
        {
            Product = product;
            Count = count;
        }
    }
}
