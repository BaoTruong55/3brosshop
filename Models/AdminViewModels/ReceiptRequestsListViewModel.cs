using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ReceiptRequestsListViewModel
    {
        public string Name { get; }
        public int Id { get; }
        public string City { get; }
        public string ReceiptName { get; }

        public ReceiptRequestsListViewModel(Receipt receipt)
        {
            Name = receipt.Seller.Name;
            ReceiptName = receipt.Name;
            City = receipt.City;
            Id = receipt.ReceiptId;
        }

    }
}
