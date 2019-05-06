﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ReceiptProcessViewModel
    {
        [Required]
        public int ReceiptId { get; set; }

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

        public ReceiptProcessViewModel(Receipt receipt)
        {
            ReceiptId = receipt.ReceiptId;
            NameSeller = receipt.Seller.Name;
            Name = receipt.Name;
            Description = receipt.Description;
            Price = receipt.Price;
            Category = receipt.Category.Name;
            Street = receipt.Street;
            ApartmentNumber = receipt.ApartmentNumber;
            Postcode = receipt.Postcode;
            City = receipt.City;
            Offer = receipt.Offer;
            GetThumbPath = "/" + receipt.GetThumbPath();
        }

        public ReceiptProcessViewModel()
        {

        }


    }
}
