using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasusBlok4.Models.Entity;
using CasusBlok4.Services;
using Microsoft.AspNetCore.Authorization;

namespace CasusBlok4.Controllers
{
    [Authorize]
    public class MyTransactionsController : Controller
    {
        private readonly DataContext _context;

        public MyTransactionsController(DataContext context)
        {
            _context = context;
        }

        // GET: MyTransactions
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Transactions;
            return View(await dataContext.ToListAsync());
        }

        // GET: MyTransactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.TransactionProducts)
                    .ThenInclude(q=>q.Product)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}
