using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ManageViewModels
{
    public class UsedOrderOverviewListViewModel
    {
        public string Date { get; }
        public decimal Amount { get; }
        public string Name { get; }

        public UsedOrderOverviewListViewModel(OrderItem order)
        {
            Date = order.ExpirationDate.ToString("dd/MM/yyyy");
            Amount = order.Price;
            Name = order.Items.Name;
        }


    }
}
