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
            if (_transactionManager.ActiveTransaction != null)
            {
                return RedirectToAction("List", "Transaction");
            }

            return View();
        }

        [HttpPost]
        public IActionResult IndexPost(TransactionIndexViewModel model)
        {
            if (_transactionManager.ActiveTransaction != null)
            {
                return RedirectToAction("List", "Transaction");
            }

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
            if (_transactionManager.ActiveTransaction == null)
            {
                return RedirectToAction("Index", "Transaction");
            }

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
            if (_transactionManager.ActiveTransaction == null)
            {
                return RedirectToAction("Index", "Transaction");
            }

            var transProducts = _dataContext.TransactionProducts.Where(q=>q.Transaction.EmployeeId == _transactionManager.ActiveTransaction.EmployeeId && q.Transaction.EndTime == null).Include(q => q.Product).AsEnumerable();
            return View(new TransactionListViewModel(transProducts, _transactionManager.ActiveTransaction.Customer.Saldo));
        }

        public IActionResult Finish()
        {
            if (_transactionManager.ActiveTransaction == null)
            {
                return RedirectToAction("Index", "Transaction");
            }

            _transactionManager.EndTransaction();

            return View();
        }
    }
}
