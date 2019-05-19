using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerEditViewModel
    {
        [Required]
        public int SellerId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string PhoneNumber { get; set; }

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
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile Image { set; get; }

        [DataType(DataType.Text)]
        [Display(Name = "Note (optional)")]
        public string Note { get; set; }

        public string LogoPath { get; }

        public SellerEditViewModel(Seller seller)
        {
            SellerId = seller.SellerId;
            Name = seller.Name;
            Email = seller.EmailAddress;
            Description = seller.Description;
            Street = seller.Street;
            ApartmentNumber = seller.ApartmentNumber;
            Postcode = seller.Postcode;
            City = seller.City;
            LogoPath = seller.GetLogoPath();
        }

        public SellerEditViewModel()
        {

        }
    }
}
