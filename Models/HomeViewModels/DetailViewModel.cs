using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class DetailViewModel
    {
        [HiddenInput]
        public int ReceiptId { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public int QuantityOrdered { get; }
        public List<string> getImagePathList { get; }
        public string SellerName { get; }
        public string SellerDescription { get; }
        public string FormatedAdress { get; }
        public string City { get; }
        public string CategoryIcon { get; }
        public string CategoryName { get; }
        public string GetThumbPath { get; }

        public DetailViewModel()
        {
        }

        public DetailViewModel(Receipt receipt)
        {
            ReceiptId = receipt.ReceiptId;
            Name = receipt.Name;
            Price = receipt.Price;
            Description = receipt.Description;
            QuantityOrdered = receipt.QuantityOrdered;
            getImagePathList = receipt.getImagePathList();
            SellerName = receipt.Seller.Name;
            SellerDescription = receipt.Seller.Description;
            FormatedAdress = receipt.Postcode + " " + receipt.City + ", " + receipt.Street + " " + receipt.ApartmentNumber;
            City = receipt.City;
            CategoryIcon = receipt.Category.Icon;
            CategoryName = receipt.Category.Name;
            GetThumbPath = receipt.GetThumbPath();
        }
    }
}
