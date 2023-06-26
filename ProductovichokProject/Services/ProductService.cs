using Microsoft.EntityFrameworkCore;
using ProductovichokProject.Data;
using ProductovichokProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Services
{
    public class ProductService
    {
        private readonly ProductovichokContext _context;

        public ObservableCollection<Cart> Cart;

        public ProductService(ProductovichokContext productovichokContext)
        {
            _context = productovichokContext;
        }

        public async Task<ObservableCollection<Product>> GetProducts()
        {
            await _context.Units.ToListAsync();
            await _context.Categories.ToListAsync();
            ObservableCollection<Product> products = new ObservableCollection<Product>(await _context.Products.ToListAsync()); 
            return products;
        }
        public async Task AddClientOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            List<Orderdetail> orderdetails = new List<Orderdetail>();
            foreach (Cart cartitem in Cart)
            {
                Orderdetail detail = new Orderdetail()
                {
                    OrderId = _context.Orders.Max(x => x.OrderId),
                    ProductId = cartitem.Product.ProductId,
                    Quantity = cartitem.Count,
                    PriceAtOrder = (int)cartitem.Product.DiscontPrice
                };
                orderdetails.Add(detail);
            }
            _context.Orderdetails.AddRange(orderdetails);
            await _context.SaveChangesAsync();
        }
    }
}
