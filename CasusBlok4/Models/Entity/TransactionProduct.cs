using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class TransactionProduct
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public bool IsForSell { get; set; }
        public int Points { get; set; }
        public int NumberOfProduct { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual Product Product { get; set; }
        public int TransactionProductId { get; internal set; }
    }
}
