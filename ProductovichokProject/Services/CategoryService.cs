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
    public class CategoryService
    {
        private readonly ProductovichokContext _context;

        public CategoryService(ProductovichokContext context)
        {
            _context = context;
        }
        
        public async Task<List<Category>> GetCategories()
        {
            return  await _context.Categories.ToListAsync();
        }
    }
}
