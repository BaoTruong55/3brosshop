using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Models.AdminViewModels
{
    public class UsersViewModel
    {
        public IdentityUser User { get; set; }
        public string RoleName { get; set; }
    }
}