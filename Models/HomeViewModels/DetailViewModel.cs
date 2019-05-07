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
        public int ItemsId { get; }
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

        public DetailViewModel(Items items)
        {
            ItemsId = items.ItemsId;
            Name = items.Name;
            Price = items.Price;
            Description = items.Description;
            QuantityOrdered = items.QuantityOrdered;
            getImagePathList = items.getImagePathList();
            SellerName = items.Seller.Name;
            SellerDescription = items.Seller.Description;
            FormatedAdress = items.Postcode + " " + items.City + ", " + items.Street + " " + items.ApartmentNumber;
            City = items.City;
            CategoryIcon = items.Category.Icon;
            CategoryName = items.Category.Name;
            GetThumbPath = items.GetThumbPath();
        }
    }
}
