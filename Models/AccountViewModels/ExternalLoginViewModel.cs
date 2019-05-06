using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Enum;

namespace Shop.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        public Sex Sex { get; set; }
    }
}
