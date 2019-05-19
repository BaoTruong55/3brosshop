using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ManageViewModels

{
    public class ItemsOverviewViewModel
    {
        public IEnumerable<ItemsListViewModel> AllItemsSortedById { get; }

        public ItemsOverviewViewModel(IEnumerable<Items> allItems)
        {
            AllItemsSortedById = allItems.Select(c => new ItemsListViewModel(c)).ToList();
        }

    }
}
