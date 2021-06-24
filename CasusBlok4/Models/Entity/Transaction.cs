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

        public int TransactionId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string SerialNumber { get; set; }
        public int ProfileId { get; set; }
        public bool IsDonation { get; set; }
        public bool IsLoan { get; set; }

        public virtual ProfileData Customer { get; set; }

        private ICollection<TransactionProduct> _transactionProducts;
        public virtual ICollection<TransactionProduct> TransactionProducts { 
            get => LazyLoader.Load(this, ref _transactionProducts); 
            set => _transactionProducts = value; 
        }
    }
}
