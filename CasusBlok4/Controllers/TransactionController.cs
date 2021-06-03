using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Controllers
{
    [Authorize(Roles = "employee")]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IndexPost()
        {
            return RedirectToAction("AddProduct");
        }
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}
