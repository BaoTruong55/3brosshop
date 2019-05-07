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
        public int ItemsId { get; }
        public string CategoryIcon { get; }
        public string CategoryName { get; }

        public IndexTop30SheetsListModel()
        {
        }

        public IndexTop30SheetsListModel(Items items)
        {
            Name = items.Name;
            Price = items.Price;
            Description = items.Description;
            QuantityOrdered = items.QuantityOrdered;
            GetThumbPath = items.GetThumbPath();
            City = items.City;
            ItemsId = items.ItemsId;
            CategoryIcon = items.Category.Icon;
            CategoryName = items.Category.Name;
        }

    }
}
