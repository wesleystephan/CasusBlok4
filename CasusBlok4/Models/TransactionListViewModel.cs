using CasusBlok4.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class TransactionListViewModel
    {


        public TransactionListViewModel(IEnumerable<TransactionProduct> transactionProducts, int currentCustomerSaldo)
        {
            CurrentCustomerSaldo = currentCustomerSaldo;
            TransactionProducts = transactionProducts;
            int saldo = 0;
            foreach (var transProduct in transactionProducts)
            {
                if (transProduct.IsForSell)
                {
                    saldo -= (transProduct.Points * transProduct.NumberOfProduct);
                }
                else
                {
                    saldo += (transProduct.Points * transProduct.NumberOfProduct);
                }
            }
            EndSaldo = saldo;
        }
        public readonly IEnumerable<TransactionProduct> TransactionProducts;
        public readonly int EndSaldo;
        public readonly int CurrentCustomerSaldo;
    }
}
