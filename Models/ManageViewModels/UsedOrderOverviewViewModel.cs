using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ManageViewModels
{
    public class UsedOrderOverviewViewModel
    {
        public IEnumerable<UsedOrderOverviewListViewModel> AllUsedOrder { get; }

        public UsedOrderOverviewViewModel(IEnumerable<OrderItem> allOrder)
        {
            AllUsedOrder = allOrder.Select(c => new UsedOrderOverviewListViewModel(c)).ToList();
        }
    }
}
