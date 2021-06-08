using CasusBlok4.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class TransactionListViewModel
    {


        public TransactionListViewModel(IEnumerable<TransactionProduct> transactionProducts, short currentCustomerSaldo)
        {
            CurrentCustomerSaldo = currentCustomerSaldo;
            TransactionProducts = transactionProducts;
            short saldo = 0;
            foreach (var transProduct in transactionProducts)
            {
                if (transProduct.IsForSell)
                {
                    saldo -= (short)(transProduct.Points * transProduct.NumberOfProduct);
                }
                else
                {
                    saldo += (short)(transProduct.Points * transProduct.NumberOfProduct);
                }
            }
            EndSaldo = saldo;
        }
        public readonly IEnumerable<TransactionProduct> TransactionProducts;
        public readonly short EndSaldo;
        public readonly short CurrentCustomerSaldo;
    }
}
