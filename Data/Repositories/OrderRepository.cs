using Shop.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbSet<Order> _orders;
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _orders = dbContext.Order;
        }

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public Order GetBy(int orderId)
        {
            return _orders.Include(bl => bl.Orders).Where(b => b.BillId == orderId).SingleOrDefault();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
