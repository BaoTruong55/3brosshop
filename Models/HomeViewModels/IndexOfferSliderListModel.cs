using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class IndexOfferSliderListModel
    {
        public string GetThumbPath { get; }
        public int QuantityOrdered { get; }
        public string Name { get; }
        public int ItemsId { get; }

        public IndexOfferSliderListModel()
        {
        }

        public IndexOfferSliderListModel(Items items)
        {
            Name = items.Name;
            QuantityOrdered = items.QuantityOrdered;
            GetThumbPath = items.GetThumbPath();
            ItemsId = items.ItemsId;
        }
    }
}
