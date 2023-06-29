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
    public class OrderService
    {
        private readonly ProductovichokContext _context;
        public Order SelectedOrder { get; set; }

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

        public async Task<List<Orderdetail>> GetOrderDetails(int orderId)
        {
            await _context.Products.ToListAsync();
            List<Orderdetail> orderdetails = await _context.Orderdetails.Where(x => x.OrderId == orderId).ToListAsync();
            return orderdetails;
        }

        public async Task<ObservableCollection<Order>> GetOrdersForPicker()
        {
            await _context.Statuses.ToListAsync();
            await _context.Addresses.ToListAsync();
            await _context.UserAddresses.ToListAsync();
            await _context.Streets.ToListAsync();
            ObservableCollection<Order> orders = new ObservableCollection<Order>(await _context.Orders.Where(x => x.StatusId == 1).ToListAsync());
            return orders;
        }

        public async Task EditOrderStatus(int statusId, Order selectedorder, int? boxnumber = null)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x == selectedorder);
            order.StatusId = statusId;
            if (boxnumber is not null)
                order.BoxNumber = (int)boxnumber;
            await _context.SaveChangesAsync();
        }

        public async Task CheckRequest(int orderId, int userId)
        {
            if(!await _context.Checks.AnyAsync(x => x.UserId == userId && x.OrderId == orderId))
            {
                Check check = new Check()
                {
                    OrderId = orderId,
                    UserId = userId
                };
                await _context.Checks.AddAsync(check);
                await _context.SaveChangesAsync();
            }
        }
    }
}
