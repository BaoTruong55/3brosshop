using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.HomeViewModels
{
    public class SendEmailViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đầy đủ")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name="Email của bạn")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề")]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nội dung")]
        public string Message { get; set; }
    }
}
