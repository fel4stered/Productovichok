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
    public class UserService
    {
        private readonly ProductovichokContext _context;

        public User UserInfo { get; set; }

        public UserService(ProductovichokContext context)
        {
            _context = context;
        }

        public async Task<bool> Authorization(string nickname, int code)
        {
            var Code = await _context.Codes.SingleOrDefaultAsync(x => x.CodeId == code && x.User.TelegramUserNickname == nickname);
            if (Code is not null)
            {
                await _context.Users.ToListAsync();
                UserInfo = Code.User;
                return true;
            }
            else
                return false;
        }

        public async Task<bool> UserAddressesChecker()
        {
            if (await _context.UserAddresses.AnyAsync(x => x.UserId == UserInfo.UserId))
                return true;
            else
                return false;
        }

        public async Task<List<UserAddress>> GetUserAddresses()
        {
            await _context.Addresses.ToListAsync();
            await _context.Streets.ToListAsync();
            await _context.Houses.ToListAsync();
            var  UserAddresses = await _context.UserAddresses.Where(x => x.UserId == UserInfo.UserId).ToListAsync();
            return UserAddresses;
        }

        public async Task AddUserAddress(UserAddress userAddress)
        {
            await _context.UserAddresses.AddAsync(userAddress);
            await _context.SaveChangesAsync();
        }
    }
}
