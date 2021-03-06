using CasusBlok4.Models;
using CasusBlok4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CasusBlok4.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[Authorize]
        //public IActionResult MyTransactions()
        //{
        //    int customerId = int.Parse(User.FindFirst(q => q.Type == ClaimTypes.NameIdentifier).Value);
        //    return View(_dataContext.Transactions.Include(q=>q.Customer).Include(q=>q.Employee).Where(q=>q.CustomerId == customerId));
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Views.Shared.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}