using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ItemsRequestsListViewModel
    {
        public string Name { get; }
        public int Id { get; }
        public string City { get; }
        public string ItemsName { get; }

        public ItemsRequestsListViewModel(Items items)
        {
            Name = items.Seller.Name;
            ItemsName = items.Name;
            City = items.City;
            Id = items.ItemsId;
        }

    }
}
