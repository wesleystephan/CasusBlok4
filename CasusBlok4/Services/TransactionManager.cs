using CasusBlok4.Models;
using CasusBlok4.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CasusBlok4.Services
{
    class TransactionManager : ITransactionManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _dataContext;
        private readonly Lazy<Transaction> _activeTransaction;

        public Transaction ActiveTransaction => _activeTransaction.Value;

        public TransactionManager(IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _contextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            
            int employeeId = int.Parse(_contextAccessor.HttpContext.User.FindFirst(q => q.Type == ClaimTypes.NameIdentifier).Value);
            _activeTransaction = new Lazy<Transaction>(dataContext.Transactions.Include(q=>q.Customer).Include(q=>q.Employee).SingleOrDefault(q => q.EmployeeId == employeeId && q.EndTime == null));
        }

        public TransactionProduct AddProductForSellToTransaction(Product product, byte numberOf, short? points) => 
            AddProductToTransaction(product, numberOf, points, true);

        public TransactionProduct AddProductToBuyToTransaction(Product product, byte numberOf, short? points) =>
            AddProductToTransaction(product, numberOf, points, false);

        public void EndTransaction()
        {
            IEnumerable<TransactionProduct> products = _dataContext.TransactionProducts.Where(q => q.TransactionId == ActiveTransaction.TransactionId);
            Customer customer = _dataContext.Customers.Find(ActiveTransaction.CustomerId);
            foreach (TransactionProduct product in products)
            {
                if (product.IsForSell)
                {
                    customer.Saldo += product.Points;
                } else
                {
                    customer.Saldo -= product.Points;
                }
            }
            ActiveTransaction.EndTime = DateTimeOffset.UtcNow;
            _dataContext.SaveChanges();
            
        }

        public Transaction StartTransaction(Customer customer)
        {
            if (_contextAccessor.HttpContext.User.Identity?.IsAuthenticated != true || !_contextAccessor.HttpContext.User.IsInRole("employee"))
            {
                throw new SecurityException("Trying to start transaction while not logged in or not having the right permissions");
            }

            int employeeId = int.Parse(_contextAccessor.HttpContext.User.FindFirst(q => q.Type == ClaimTypes.NameIdentifier).Value);
            if (_dataContext.Transactions.Any(q=>q.EmployeeId == employeeId && q.EndTime == null))
            {
                throw new InvalidOperationException("Cannot start transaction while there is one open");
            }


            Transaction transaction = new Transaction()
            {
                TransactionId = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
                CustomerId = customer.Id,
                StartTime = DateTimeOffset.UtcNow,
            };
            _dataContext.Transactions.Add(transaction);

            _dataContext.SaveChanges();
            return transaction;
        }

        private TransactionProduct AddProductToTransaction(Product product, byte numberOf, short? points, bool isForSell)
        {
            short? sellPoints = points ?? product.PointsWorth;

            if (!sellPoints.HasValue)
            {
                throw new InvalidOperationException("There is no default points for this product, while no points were given");
            }

            TransactionProduct transactionProduct = new TransactionProduct()
            {
                IsForSell = isForSell,
                ProductId = product.Id,
                Points = sellPoints.Value,
                NumberOfProduct = numberOf,
                TransactionId = ActiveTransaction.TransactionId,
            };
            _dataContext.TransactionProducts.Add(transactionProduct);
            _dataContext.SaveChanges();

            return transactionProduct;
        }
    }

    public interface ITransactionManager
    {
        public Transaction ActiveTransaction { get; }
        public Transaction StartTransaction(Customer customer);
        public TransactionProduct AddProductForSellToTransaction(Product product, byte numberOf, short? points);
        public TransactionProduct AddProductToBuyToTransaction(Product product, byte numberOf, short? points);
        public void EndTransaction();
    }
}
