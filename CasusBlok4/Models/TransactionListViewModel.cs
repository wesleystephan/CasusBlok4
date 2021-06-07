using CasusBlok4.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class TransactionListViewModel
    {
        public IEnumerable<TransactionProduct> TransactionProducts;
        public short CalculateEndSaldo() => (short)TransactionProducts.Sum(q => q.Points * (q.IsForSell ? 1 : -1));
    }
}
