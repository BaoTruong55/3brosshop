using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ShoppingCartViewModels
{
    public class ThanksViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

    }
}
