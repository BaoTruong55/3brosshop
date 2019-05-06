using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.AccountViewModels
{
    public class RegisterHandelaarViewModel
    {

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [Display(Name = "Tên")]
        [DataType(DataType.Text)]
        public string NameOfBusiness { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đường")]
        public string Street { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Số nhà")]
        public string ApartmentNumber { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Thành Phố")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Upload)]
        [Display(Name = "Logo")]
        public IFormFile Logo { set; get; }
    }
    
}
