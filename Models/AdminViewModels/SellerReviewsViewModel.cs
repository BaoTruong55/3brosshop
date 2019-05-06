using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerReviewsViewModel
    {
        public IEnumerable<SellerRequestListViewModel> AllDealersRequestsSortedById { get; }

        public SellerReviewsViewModel(IEnumerable<Seller> AllSellerYetNotApproved)
        {
            AllDealersRequestsSortedById = AllSellerYetNotApproved.Select(h => new SellerRequestListViewModel(h)).ToList();
        }

    }
}
