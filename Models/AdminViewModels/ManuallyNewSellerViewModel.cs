using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shop.Models.AdminViewModels
{
    public class ManuallyNewSellerViewModel
    {
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
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đường")]
        public string StreetName { get; set; }

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
        [Display(Name = "Thành Phố")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Hình ảnh")]
        public IFormFile Image { set; get; }

        [DataType(DataType.Text)]
        [Display(Name = "Lưu ý (tùy chọn)")]
        public string Note { get; set; }


    }
}
