using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerOverviewViewModel
    {
        public IEnumerable<SellerListViewModel> AllSellerSortedById { get; }

        public SellerOverviewViewModel(IEnumerable<Seller> allSeller)
        {
            AllSellerSortedById = allSeller.Reverse().Select(h => new SellerListViewModel(h)).ToList();
        }

    }
}
