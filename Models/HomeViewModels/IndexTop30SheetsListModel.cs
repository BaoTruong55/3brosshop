using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class IndexTop30SheetsListModel
    {

        
        public string GetThumbPath { get; }
        public string City { get; }
        public int QuantityOrdered { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public int ReceiptId { get; }
        public string CategoryIcon { get; }
        public string CategoryName { get; }

        public IndexTop30SheetsListModel()
        {
        }

        public IndexTop30SheetsListModel(Receipt receipt)
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
