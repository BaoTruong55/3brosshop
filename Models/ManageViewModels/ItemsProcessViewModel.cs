using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.ManageViewModels
{
    public class ItemsProcessViewModel
    {
        [Required]
        public int ItemsId { get; set; }

        [Required]
        public string NameSeller { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Thể loại")]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đường")]
        public string Street { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Số nhà")]
        public string ApartmentNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thành phố")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Offer")]
        public Offer Offer { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Hình thu nhỏ")]
        public IFormFile Thumbnail { set; get; }

        [DataType(DataType.Upload)]
        [Display(Name = "Hình ảnh")]
        public List<IFormFile> Image { set; get; }

        [DataType(DataType.Text)]
        [Display(Name = "Lưu ý(tùy chọn)")]
        public string Note { get; set; }

        public string GetThumbPath { get; }

        public ItemsProcessViewModel(Items items)
        {
            ItemsId = items.ItemsId;
            NameSeller = items.Seller.Name;
            Name = items.Name;
            Description = items.Description;
            Price = items.Price;
            Category = items.Category.Name;
            Street = items.Street;
            ApartmentNumber = items.ApartmentNumber;
            Postcode = items.Postcode;
            City = items.City;
            Offer = items.Offer;
            GetThumbPath = "/" + items.GetThumbPath();
        }

        public ItemsProcessViewModel()
        {

        }


    }
}
