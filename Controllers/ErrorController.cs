using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class ErrorController : Controller
    {

        [Route("404")]
        public IActionResult PageNotFound()
        {
            // Perform any action before serving the View
            return View();
        }
    }
}