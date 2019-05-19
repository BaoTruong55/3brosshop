using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain.Interface
{
    public interface IUserRepository
    {
        User GetBy(string email);
        User GetByOrderId(int orderId);
        void Add(User user);
        void SaveChanges();
        void SaveChangesAsync();
    }
}
