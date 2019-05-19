using Shop.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.User;
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User GetBy(string email)
        {
            return _users.Include(g => g.Bills).SingleOrDefault(g => g.EmailAddress == email);
        }

        public User GetByOrderId(int orderId)
        {
            return _users.Include(g => g.Bills).SingleOrDefault(g => g.Bills.Select(b => b.BillId).Contains(orderId));
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void SaveChangesAsync()
        {
            _dbContext.SaveChangesAsync();
        }

        
    }
}
