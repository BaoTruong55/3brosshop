using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain.Interface
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Order GetBy(int orderId);
        void SaveChanges();
    }
}
