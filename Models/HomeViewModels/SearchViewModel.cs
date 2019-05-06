using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public string City { get; }
        public string CategoryIcon { get; }
        public string CategoryName { get; }
        public int QuantityOrdered { get; }
        public int ReceiptId { get; }
        public string GetThumbPath { get; }

        public SearchViewModel()
        {
        }

        public SearchViewModel(Receipt receipt)
        {
            Name = receipt.Name;
            Price = receipt.Price;
            Description = receipt.Description;
            QuantityOrdered = receipt.QuantityOrdered;
            GetThumbPath = receipt.GetThumbPath();
            City = receipt.City;
            ReceiptId = receipt.ReceiptId;
            CategoryIcon = receipt.Category.Icon;
            CategoryName = receipt.Category.Name;
        }
    }
}
