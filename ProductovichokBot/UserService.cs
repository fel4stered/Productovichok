using Microsoft.EntityFrameworkCore;
using ProductovichokBot.Data;
using ProductovichokBot.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokBot
{
    internal class UserService
    {
        internal static readonly ProductovichokContext _context = new ProductovichokContext();

        internal static bool RegistrationCheck(int userid)
        {
            if (_context.Users.SingleOrDefault(x => x.UserId == userid) is not null)
                return true;
            else
                return false;
        }
        internal async static void Registration(int userid, string? nickname, string? name)
        {
            await _context.Users.AddAsync(new User()
            {
                UserId = userid,
                TelegramUserNickname = nickname,
                FirstName = name,
            });
            await _context.SaveChangesAsync();
        }
        internal static async void Authorization(int userid, int code)
        {
            await _context.Codes.AddAsync(new Code()
            {
                UserId = userid,
                CodeId = code,
            });
            await _context.SaveChangesAsync();
        }
        internal static bool AuthorizationCheck(int userid)
        {
            if (_context.Codes.SingleOrDefaultAsync(x => x.UserId == userid).Result is not null)
                return true;
            else
                return false;

        }
    }
}
