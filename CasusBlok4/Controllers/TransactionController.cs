using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasusBlok4.Models;
using CasusBlok4.Services;
using CasusBlok4.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CasusBlok4.Controllers
{
    [Authorize(Roles = "employee")]
    public class TransactionController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ITransactionManager _transactionManager;

        public TransactionController(DataContext dataContext, ITransactionManager transactionManager)
        {
            _dataContext = dataContext;
            _transactionManager = transactionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IndexPost(TransactionIndexViewModel model)
        {
            Customer customer = _dataContext.Customers.Find(model.CustomerId);

            if (customer == null)
            {
                ModelState.AddModelError(nameof(model.CustomerId), "Onbekende klant id");
                return View("Index");
            }

            _transactionManager.StartTransaction(customer);
            return RedirectToAction("AddProduct");
        }
        public IActionResult AddProduct()
        {
            return View(new TransactionAddProductViewModel()
            {
                Products = _dataContext.Products
            });
        }
        
        [HttpPost]
        public IActionResult AddProductPost(TransactionAddProductViewModel model)
        {
            Product product = _dataContext.Products.Find(model.SelectedProductId.ToString());
            if (product == null)
            {
                ModelState.AddModelError(nameof(model.SelectedProductId), "Onbekend product");
                return View("AddProduct", new TransactionAddProductViewModel()
                {
                    Products = _dataContext.Products
                });
            }

            if (model.IsForSell)
            {
                _transactionManager.AddProductForSellToTransaction(product, model.NumberOfProducts, model.Points);
            } else
            {
                _transactionManager.AddProductToBuyToTransaction(product, model.NumberOfProducts, model.Points);
            }
            
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var transProducts = _dataContext.TransactionProducts.Include(q => q.Product).AsEnumerable();
            return View(new TransactionListViewModel()
            {
                TransactionProducts = transProducts
            });
        }

        public IActionResult Finish()
        {
            //if transacion is active complete...
            return View();

            //else otherwise redirect


        }
    }
}
