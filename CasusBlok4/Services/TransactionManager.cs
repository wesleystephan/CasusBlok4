using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Services
{
    class TransactionManager : ITransactionManager
    {
        public void AddProductForSellToTransaction()
        {
            throw new NotImplementedException();
        }

        public void AddProductToBuyToTransaction()
        {
            throw new NotImplementedException();
        }

        public void EndTransaction()
        {
            throw new NotImplementedException();
        }

        public void StartTransaction()
        {
            throw new NotImplementedException();
        }
    }

    public interface ITransactionManager
    {
        public void StartTransaction();
        public void AddProductForSellToTransaction();
        public void AddProductToBuyToTransaction();
        public void EndTransaction();
    }
}
