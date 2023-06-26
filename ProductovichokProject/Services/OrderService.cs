using Microsoft.EntityFrameworkCore;
using ProductovichokProject.Data;
using ProductovichokProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Services
{
    public class OrderService
    {
        private readonly ProductovichokContext _context;

        public OrderService(ProductovichokContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrders(int userId)
        {
            await _context.Statuses.ToListAsync();  
            List<Order> orders = await _context.Orders.Where(x => x.ClientId == userId).ToListAsync();
            return orders;
        }
    }
}
