using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ItemsRequestsViewModel
    {
        public IEnumerable<ItemsRequestsListViewModel> AllItemsRequestsSortedById { get; }

        public ItemsRequestsViewModel(IEnumerable<Items> AllItemsNotYetApproved)
        {
            AllItemsRequestsSortedById = AllItemsNotYetApproved.Select(b => new ItemsRequestsListViewModel(b)).ToList();
        }

    }
}
