using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class TransactionProduct
    {
        public string TransactionId { get; set; }
        public string ProductId { get; set; }
        public bool IsForSell { get; set; }
        public short Points { get; set; }
        public byte NumberOfProduct { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual Product Product { get; set; }
    }
}
