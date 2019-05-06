using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ManageViewModels
{
    public class IndexSellerViewModel
    {
        [Display(Name = "Tên")]
        public string Name { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đường")]
        public string Street { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Thành phố")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        public string Postcode { get; set; }

        public string StatusMessage { get; set; }
    }
}
