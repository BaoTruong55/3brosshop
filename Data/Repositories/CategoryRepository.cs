using Shop.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _categories = context.Category;
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetByName(string naam)
        {
            return _categories.SingleOrDefault(c => c.Name.ToLower() == naam.ToLower());
        }
       

        public Dictionary<Category, int> GetTop9WithAmount()
        {
            var map = new Dictionary<Category, int>();
            var categories = _categories.Include(c => c.Coupons).OrderByDescending(c => c.Coupons.Count).ThenBy(c => c.Name).Take(9);

            foreach (Category cat in categories)
            {
                map.Add(cat, cat.Coupons.Count);
            }

            return map;
        }
    }
}
