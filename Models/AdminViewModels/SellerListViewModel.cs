using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerListViewModel
    {
        public int Id { get; }
        public string City { get; }
        public string Name { get; }
        public string Postcode { get; }
        public int NumberOfItemsInSystem { get; }

        public SellerListViewModel(Seller seller)
        {
            Id = seller.SellerId;
            City = seller.City;
            Name = seller.Name;
            NumberOfItemsInSystem = seller.Items.Count;
            Postcode = seller.Postcode;
        }


    }
}
