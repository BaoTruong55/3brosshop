using Shop.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ShoppingCartViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<IndexOrderListViewModel> ShoppingCartList { get; }
        public decimal Total { get; }

        public IndexViewModel(IEnumerable<ShoppingCartItem> cartList, decimal total)
        {
            ShoppingCartList = cartList.Select(wl => new IndexOrderListViewModel(wl)).ToList();
            Total = total;
        }
    }

}
