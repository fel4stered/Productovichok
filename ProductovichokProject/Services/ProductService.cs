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

        public ProductService(ProductovichokContext productovichokContext)
        {
            _context = productovichokContext;
        }

        public async Task<ObservableCollection<Product>> GetProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>(_context.Products.ToList()); 
            return products;
        }
    }
}
