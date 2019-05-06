using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class OverviewUsedOrderViewModel
    {
        public IEnumerable<OverviewSoldOrderListViewModel> UsedOrderList { get; }

        public OverviewUsedOrderViewModel(IEnumerable<OrderItem> usedOrder)
        {
            UsedOrderList = usedOrder.OrderByDescending(b => b.ExpirationDate).Select(b => new OverviewSoldOrderListViewModel(b)).ToList();
        }

    }
}
