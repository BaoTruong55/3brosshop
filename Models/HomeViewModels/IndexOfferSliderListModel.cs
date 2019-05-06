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
        public int ReceiptId { get; }

        public IndexOfferSliderListModel()
        {
        }

        public IndexOfferSliderListModel(Receipt receipt)
        {
            Name = receipt.Name;
            QuantityOrdered = receipt.QuantityOrdered;
            GetThumbPath = receipt.GetThumbPath();
            ReceiptId = receipt.ReceiptId;
        }
    }
}
