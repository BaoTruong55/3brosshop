using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain.Interface
{
    public interface IOrderItemRepository
    {
        void Add(OrderItem orderItem);
        OrderItem GetBy(string qrcode);
        IEnumerable<OrderItem> GetAll();
        IEnumerable<OrderItem> getSoldThisMonth();
        IEnumerable<OrderItem> getUsedThisMonth();
        IEnumerable<OrderItem> getUsedOrder();
        IEnumerable<OrderItem> getUsedItemsFromSellerId(int id);
        IEnumerable<OrderItem> getSoldOrder();
        void SaveChanges();
        OrderItem GetById(int orderItemId);
    }
}
