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
        public int ItemsId { get; }

        public int Count { get; }

        public string ItemsName { get; }

        public String GetThumbPath { get; }

        public string CategoryName { get; }

        public decimal Price { get; }

        public decimal Total { get; }

        public IndexOrderListViewModel(ShoppingCartItem shoppingCartItem)
        {
            ItemsId = shoppingCartItem.Items.ItemsId;
            Count = shoppingCartItem.Count;
            ItemsName = shoppingCartItem.Items.Name;
            CategoryName = shoppingCartItem.Items.Category.Name;
            Price = shoppingCartItem.Price;
            Total = shoppingCartItem.Total;
            GetThumbPath = shoppingCartItem.Items.GetThumbPath();
        }
    }
}
