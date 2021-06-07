using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class Transaction
    {
        public Transaction(){}
        public Transaction(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public string TransactionId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }

        private ICollection<TransactionProduct> _transactionProducts;
        public virtual ICollection<TransactionProduct> TransactionProducts { 
            get => LazyLoader.Load(this, ref _transactionProducts); 
            set => _transactionProducts = value; 
        }
    }
}
