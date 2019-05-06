using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ShoppingCartViewModels
{
    public class IndexOrderListViewModel
    {
        [HiddenInput]
        public int ReceiptId { get; }

        public int Count { get; }

        public string ReceiptName { get; }

        public String GetThumbPath { get; }

        public string CategoryName { get; }

        public decimal Price { get; }

        public decimal Total { get; }

        public IndexOrderListViewModel(ShoppingCartItem shoppingCartItem)
        {
            ReceiptId = shoppingCartItem.Receipt.ReceiptId;
            Count = shoppingCartItem.Count;
            ReceiptName = shoppingCartItem.Receipt.Name;
            CategoryName = shoppingCartItem.Receipt.Category.Name;
            Price = shoppingCartItem.Price;
            Total = shoppingCartItem.Total;
            GetThumbPath = shoppingCartItem.Receipt.GetThumbPath();
        }
    }
}
