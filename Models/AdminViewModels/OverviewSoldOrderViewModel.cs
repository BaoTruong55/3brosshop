using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class OverviewSoldOrderViewModel
    {
        public IEnumerable<OverviewSoldOrderListViewModel> SoldOrderList { get; }

        public OverviewSoldOrderViewModel(IEnumerable<OrderItem> orderSold)
        {
            SoldOrderList = orderSold.OrderByDescending(b => b.CreationDate).Select(b => new OverviewSoldOrderListViewModel(b)).ToList();
        }

    }
}
