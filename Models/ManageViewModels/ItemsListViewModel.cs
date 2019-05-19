using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ManageViewModels
{
    public class ItemsListViewModel
    {
        public string Name { get; }
        public int Id { get; }
        public string Ciy { get; }
        public string ItemsName { get; }

        public int NumberOfItemsInSystem { get; }

        public ItemsListViewModel(Items items)
        {
            Name = items.Seller.Name;
            ItemsName = items.Name;
            Ciy = items.City;
            Id = items.ItemsId;
            NumberOfItemsInSystem = items.QuantityOrdered;
        }


    }
}
