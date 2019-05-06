using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ReceiptListViewModel
    {
        public string Name { get; }
        public int Id { get; }
        public string Ciy { get; }
        public string ReceiptName { get; }

        public int NumberofReceiptinSystem { get; }

        public ReceiptListViewModel(Receipt receipt)
        {
            Name = receipt.Seller.Name;
            ReceiptName = receipt.Name;
            Ciy = receipt.City;
            Id = receipt.ReceiptId;
            NumberofReceiptinSystem = receipt.QuantityOrdered;
        }


    }
}
