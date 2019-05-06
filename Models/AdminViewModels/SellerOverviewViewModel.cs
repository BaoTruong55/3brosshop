using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerOverviewViewModel
    {
        public IEnumerable<SellerListViewModel> AllSellerSortedOpId { get; }

        public SellerOverviewViewModel(IEnumerable<Seller> allSeller)
        {
            AllSellerSortedOpId = allSeller.Reverse().Select(h => new SellerListViewModel(h)).ToList();
        }

    }
}
