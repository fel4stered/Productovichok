using Microsoft.EntityFrameworkCore;
using ProductovichokProject.Data;
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

        public UserService(ProductovichokContext context)
        {
            _context = context;
        }

        public async Task<bool> Authorization(string nickname, int code)
        {
            var Code = await _context.Codes.SingleOrDefaultAsync(x => x.CodeId == code && x.User.TelegramUserNickname == nickname);
            if (Code is not null)
                return true;
            else
                return false;
        }
    }
}
