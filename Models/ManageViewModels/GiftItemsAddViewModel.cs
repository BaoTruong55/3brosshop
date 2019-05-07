using Shop.Models.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ManageViewModels
{
    public class GiftItemsAddViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nội dung")]
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
        [DataType(DataType.Upload)]
        [Display(Name = "Hình thu nhỏ")]
        public IFormFile Thumbnail { set; get; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Hình ảnh")]
        public List<IFormFile> Image { set; get; }


    }
}
