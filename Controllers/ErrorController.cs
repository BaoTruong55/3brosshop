using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop.Models.Domain.Interface;

namespace Shop.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public ErrorController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            var statusCodeData = HttpContext.Features.Get < IStatusCodeReExecuteFeature > ();
            switch (statusCode)
            {
                case 404:
            
                    ViewBag.ErrorCode = "404";
                    ViewBag.ErrorMessage = "Hình như địa chỉ trang web bị sai";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Xin lỗi đã bị vấn đề kết nối tới server";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
            break;
        }
        return View();
    }
}
}